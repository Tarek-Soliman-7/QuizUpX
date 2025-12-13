using Domain.Contracts;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Services.Abstraction.Contracts;
using Shared.Dtos;
using System.Text.Json;

namespace Services.Implementations
{
    public class QuizService : IQuizService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly IQuestionRepository _questionRepo;
        private readonly IAttemptRepository _attemptRepo;
        private readonly IAttemptAnswerRepository _attemptAnswerRepo;
        private readonly IUnitOfWork _unitOfWork;

        public QuizService(
            IStudentRepository studentRepo,
            IQuestionRepository questionRepo,
            IAttemptRepository attemptRepo,
            IAttemptAnswerRepository attemptAnswerRepo,
            IUnitOfWork unitOfWork)
        {
            _studentRepo = studentRepo;
            _questionRepo = questionRepo;
            _attemptRepo = attemptRepo;
            _attemptAnswerRepo = attemptAnswerRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<StudentLoginResponseDto> LoginAsync(StudentLoginRequestDto dto)
        {
            var student = await _studentRepo
                .GetByUniversityCodeAsync(dto.StudentCode);

            if (student == null || student.Pin != dto.Pin)
            {
                return new StudentLoginResponseDto
                {
                    Success = false,
                    Message = "Invalid code or pin"
                };
            }

            return new StudentLoginResponseDto
            {
                Success = true,
                StudentId = student.Id,
                StudentName = student.Name,
                Message = "Login successful"
            };
        }

        public async Task<List<QuestionDto>> GetQuestionsAsync(int subjectId)
        {
            var questions = await _questionRepo
                .GetBySubjectIdAsync(subjectId);

            return questions.Select(q => new QuestionDto
            {
                Id = q.Id,
                Title = q.Title,
                Choices = q.Choices,
                CorrectIndex = q.CorrectIndex, // مؤقتًا
                Mark = q.Mark
            }).ToList();
        }

        public async Task<SubmitResultDto> SubmitQuizAsync(SubmitQuizDto dto)
        {
            var attempt = await _attemptRepo
                .GetByStudentAndSubjectAsync(dto.StudentId, dto.SubjectId);

            if (attempt == null)
            {
                attempt = new Attempt
                {
                    StudentId = dto.StudentId,
                    SubjectId = dto.SubjectId,
                    StartedAt = DateTime.UtcNow
                };

                await _attemptRepo.AddAsync(attempt);
                await _unitOfWork.CompleteAsync();
            }

            int totalScore = 0;
            int correctAnswers = 0;

            foreach (var ans in dto.Answers)
            {
                var question = await _questionRepo.GetByIdAsync(ans.QuestionId);
                if (question == null) continue;

                bool isCorrect = ans.SelectedIndex == question.CorrectIndex;

                if (isCorrect)
                {
                    totalScore += question.Mark;
                    correctAnswers++;
                }

                var attemptAnswer = new AttemptAnswer
                {
                    AttemptId = attempt.Id,
                    QuestionId = question.Id,
                    SelectedIndex = ans.SelectedIndex,
                    IsCorrect = isCorrect
                };

                await _attemptAnswerRepo.AddAsync(attemptAnswer);
            }

            attempt.SubmittedAt = DateTime.UtcNow;
            attempt.TotalScore = totalScore;
            attempt.CorrectAnswers = correctAnswers;

            await _unitOfWork.CompleteAsync();

            return new SubmitResultDto
            {
                TotalScore = totalScore,
                CorrectAnswers = correctAnswers,
                IncorrectAnswers = dto.Answers.Count - correctAnswers
            };
        }

        public async Task<List<QuestionResultDto>> GetReviewAsync(int attemptId)
        {
            var answers = await _attemptAnswerRepo
                .GetByAttemptIdAsync(attemptId);

            return answers.Select(a => new QuestionResultDto
            {
                Question = a.Question.Title,
                CorrectAnswer = a.Question.Choices[a.Question.CorrectIndex],
                StudentAnswer = a.Question.Choices[a.SelectedIndex],
                IsCorrect = a.IsCorrect
            }).ToList();
        }

    }
}
