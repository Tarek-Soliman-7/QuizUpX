using Domain.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Services.Abstraction.Contracts;
using Shared.Dtos;
using System.Text.Json;

namespace Services.Implementations
{
    public class ExamService : IExamService
    {
        private readonly IQuestionRepository _questionRepo;
        private readonly IAttemptRepository _attemptRepo;
        private readonly IStudentService _studentService;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<ExamService> _logger;

        public ExamService(
            IQuestionRepository questionRepo,
            IAttemptRepository attemptRepo,
            IStudentService studentService,
        IUnitOfWork uow,
            ILogger<ExamService> logger)
        {
            _questionRepo = questionRepo;
            _attemptRepo = attemptRepo;
            _studentService = studentService;
            _uow = uow;
            _logger = logger;
        }

        public Task<SubmitResultDto?> GetAttemptResultAsync(int attemptId, bool includeCorrectAnswers = false)
        {
            throw new NotImplementedException();
        }

        public async Task<SubmitResultDto> GradeAndSaveAttemptAsync(SubmitDto dto, string? userId = null, string? ipAddress = null, string? deviceInfo = null)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            // load questions for the subject
            var questions = (await _questionRepo.GetBySubjectAsync(dto.subjectId)).ToList();
            if (questions == null || questions.Count == 0)
                throw new InvalidOperationException("No questions found for the subject.");

            int totalMark = 0;
            int score = 0;
            int correctCount = 0;
            var details = new List<QuestionResultDto>();

            // answers now expected as Dictionary<int,int> (questionId -> selectedIndex)
            var answers = dto.answers ?? new Dictionary<int, int>();

            foreach (var q in questions)
            {
                totalMark += q.mark;
                bool isCorrect = false;

                if (!answers.TryGetValue(q.id, out var studentIndex))
                {
                    details.Add(new QuestionResultDto
                    {
                        id = q.id,
                        isCorrect = false,
                        mark = 0,
                        correctIndex = q.correctIndex
                    });
                    continue;
                }

                // For MCQ and for True/False (where we also use index: 0 => True, 1 => False)
                if (studentIndex >= 0 && studentIndex == q.correctIndex)
                {
                    isCorrect = true;
                }

                var awarded = isCorrect ? q.mark : 0;
                if (isCorrect) { correctCount++; score += awarded; }

                details.Add(new QuestionResultDto
                {
                    id = q.id,
                    isCorrect = isCorrect,
                    mark = awarded,
                    correctIndex = q.correctIndex
                });
            }

            var result = new SubmitResultDto
            {
                TotalQuestions = questions.Count,
                CorrectCount = correctCount,
                Score = score,
                MaxScore = totalMark,
                Details = details
            };

            // create and save attempt
            var attempt = new Attempt
            {
                SubjectId = dto.subjectId,
                UserId = userId,
                StartedAt = dto.startedAt ?? DateTime.UtcNow.AddSeconds(-(dto.timeTakenSeconds ?? 0)),
                SubmittedAt = DateTime.UtcNow,
                TotalQuestions = result.TotalQuestions,
                CorrectCount = result.CorrectCount,
                Score = result.Score,
                MaxScore = result.MaxScore,
                TimeTakenSeconds = dto.timeTakenSeconds ?? 0,
                AnswersJson = JsonSerializer.Serialize(dto.answers),
                IpAddress = ipAddress,
                DeviceInfo = deviceInfo,
                Status = "Submitted"
            };

            await _attemptRepo.AddAsync(attempt);
            await _uow.CommitAsync();

            result.AttemptId = attempt.Id;

            return result;
        }

        public async Task<StartQuizResponse> StartExamAsync(StartQuizRequest req)
        {
            // 1. verify subject exists via questions repo (or subject repo)
            // 2. verify pin if required
            if (!string.IsNullOrWhiteSpace(req.Pin))
            {
                var ok = await _studentService.VerifyPinAsync(req.UniversityCode, req.Pin);
                if (!ok)
                    throw new UnauthorizedAccessException("Invalid PIN or locked.");
            }

            var questions = (await _questionRepo.GetBySubjectAsync(req.SubjectId)).ToList();
            if (!questions.Any())
                throw new InvalidOperationException("No questions for subject.");

            // shuffle
            var rng = new Random();
            var shuffled = questions.OrderBy(_ => rng.Next()).ToList();

            if (req.Take.HasValue)
                shuffled = shuffled.Take(req.Take.Value).ToList();

            // create attempt
            var attempt = new Attempt
            {
                SubjectId = req.SubjectId,
                UniversityCode = req.UniversityCode,
                StartedAt = DateTime.UtcNow,
                Status = "InProgress",
                TotalQuestions = shuffled.Count,
                TimeTakenSeconds = 0
            };

            await _attemptRepo.AddAsync(attempt);
            await _uow.CommitAsync();

            // build DTO questions (do NOT include CorrectIndex)
            var dtoQuestions = shuffled.Select(q => new QuestionDto
            {
                id = q.id,
                subjectId = q.subjectId,
                title = q.title,
                choices = q.choices.ToList(), // adapt to your Choice model
                questionType = q.questionType,
                mark = q.mark
            }).ToList();

            return new StartQuizResponse
            {
                AttemptId = attempt.Id,
                SubjectId = req.SubjectId,
                StartedAt = attempt.StartedAt,
                Questions = dtoQuestions
            };
        }

        public async Task<SubmitResultDto> SubmitAttemptAsync(SubmitResultDto req, string? userId = null)
        {
            var attempt = await _attemptRepo.GetByIdAsync(req.AttemptId);
            if (attempt == null) throw new KeyNotFoundException("Attempt not found.");
            if (attempt.Status == "Submitted") throw new InvalidOperationException("Attempt already submitted.");

            // Get relevant questions from subject (or load specific question Ids)
            var questions = (await _questionRepo.GetBySubjectAsync(attempt.SubjectId)).ToList();

            // Map questions by id for fast lookup
            var dictQ = questions.ToDictionary(q => q.id);

            int score = 0;
            int maxScore = 0;
            int correctCount = 0;
            var details = new List<QuestionResultDto>();

            foreach (var q in questions)
            {
                maxScore += q.mark;
                if (!req.answers.TryGetValue(q.id, out var studentIndex))
                {
                    // unanswered
                    details.Add(new QuestionResultDto
                    {
                        id = q.id,
                        isCorrect = false,
                        mark = 0,
                        correctIndex = q.correctIndex // your property name
                    });
                    continue;
                }

                bool isCorrect = studentIndex == q.correctIndex;
                var awarded = isCorrect ? q.mark : 0;
                if (isCorrect) { correctCount++; score += awarded; }

                details.Add(new QuestionResultDto
                {
                    id = q.id,
                    isCorrect = isCorrect,
                    mark = awarded,
                    correctIndex = q.correctIndex
                });
            }

            // update attempt
            attempt.SubmittedAt = DateTime.UtcNow;
            attempt.Score = score;
            attempt.MaxScore = maxScore;
            attempt.CorrectCount = correctCount;
            attempt.AnswersJson = JsonSerializer.Serialize(req.answers);
            //attempt.TimeTakenSeconds = re
            attempt.Status = "Submitted";
            attempt.UserId = userId;

            _attemptRepo.UpdateAsync(attempt);
            await _uow.CommitAsync();

            return new SubmitResultDto
            {
                AttemptId = attempt.Id,
                Score = score,
                MaxScore = maxScore,
                CorrectCount = correctCount,
                TotalQuestions = attempt.TotalQuestions,
                Details = details
            };
        }
    }
}
