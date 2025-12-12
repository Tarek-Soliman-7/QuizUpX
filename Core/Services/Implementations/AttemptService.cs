using Domain.Contracts;
using Microsoft.Extensions.Logging;
using Services.Abstraction.Contracts;
using Shared.Dtos;
using System.Text.Json;
using System;
using Domain.Entities;

namespace Services.Implementations
{
    public class AttemptService: IAttemptService
    {
        private readonly IAttemptRepository _attemptRepo;
        private readonly IQuestionRepository _questionRepo;
        private readonly IUnitOfWork _uow;
        private readonly ILogger<AttemptService> _logger;

        public AttemptService(
            IAttemptRepository attemptRepo,
            IQuestionRepository questionRepo,
            IUnitOfWork uow,
            ILogger<AttemptService> logger)
        {
            _attemptRepo = attemptRepo;
            _questionRepo = questionRepo;
            _uow = uow;
            _logger = logger;
        }

        public async Task<AttemptDto> GetAttemptSummaryAsync(int attemptId)
        {
            var a = await _attemptRepo.GetByIdAsync(attemptId);
            if (a == null) throw new KeyNotFoundException($"Attempt {attemptId} not found");

            return new AttemptDto
            {
                Id = a.Id,
                SubjectId = a.SubjectId,
                SubmittedAt = a.SubmittedAt,
                Score = a.Score,
                MaxScore = a.MaxScore
            };
        }

        public async Task<AttemptDetailsDto> GetAttemptDetailsAsync(int attemptId)
        {
            var a = await _attemptRepo.GetByIdAsync(attemptId);
            if (a == null) throw new KeyNotFoundException($"Attempt {attemptId} not found");

            var details = new AttemptDetailsDto
            {
                Id = a.Id,
                SubjectId = a.SubjectId,
                SubmittedAt = a.SubmittedAt,
                Score = a.Score,
                MaxScore = a.MaxScore,
                AnswersJson = a.AnswersJson,
                Details = new List<QuestionResultDto>()
            };

            try
            {
                var answers = JsonSerializer.Deserialize<Dictionary<int, JsonElement>>(a.AnswersJson) ?? new();
                // use questionRepo to get questions for the subject
                var questions = await _questionRepo.GetBySubjectAsync(a.SubjectId);

                foreach (var q in questions)
                {
                    bool isCorrect = false;
                    int awarded = 0;

                    if (answers.TryGetValue(q.id, out var jsonAnswer))
                    {
                        // MCQ
                        if (!q.questionType)
                        {
                            int studentIndex = -1;
                            if (jsonAnswer.ValueKind == JsonValueKind.Number && jsonAnswer.TryGetInt32(out var n)) studentIndex = n;
                            else if (jsonAnswer.ValueKind == JsonValueKind.String && int.TryParse(jsonAnswer.GetString(), out var idx)) studentIndex = idx;

                            if (studentIndex >= 0 && studentIndex == q.correctIndex) { isCorrect = true; awarded = q.mark; }
                        }
                        else // True/False
                        {
                            bool studentBool = false;
                            if (jsonAnswer.ValueKind == JsonValueKind.True) studentBool = true;
                            else if (jsonAnswer.ValueKind == JsonValueKind.False) studentBool = false;
                            else if (jsonAnswer.ValueKind == JsonValueKind.Number && jsonAnswer.TryGetInt32(out var nb)) studentBool = nb != 0;
                            else if (jsonAnswer.ValueKind == JsonValueKind.String && bool.TryParse(jsonAnswer.GetString(), out var bb)) studentBool = bb;

                            bool correctBool = q.correctIndex == 0;
                            if (studentBool == correctBool) { isCorrect = true; awarded = q.mark; }
                        }
                    }

                    details.Details.Add(new QuestionResultDto
                    {
                        id = q.id,
                        isCorrect = isCorrect,
                        mark = awarded,
                        correctIndex = q.correctIndex
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to build attempt details for attempt {AttemptId}", attemptId);
            }

            return details;
        }

        public async Task<IReadOnlyList<AttemptDto>> GetUserAttemptsAsync(string userId, int skip = 0, int take = 50)
        {
            var list = await _attemptRepo.GetByUserIdAsync(userId, take: take, skip: skip);
            return list.Select(a => new AttemptDto
            {
                Id = a.Id,
                SubjectId = a.SubjectId,
                SubmittedAt = a.SubmittedAt,
                Score = a.Score,
                MaxScore = a.MaxScore
            }).ToList();
        }

        public async Task DeleteAttemptAsync(int attemptId)
        {
            var a = await _attemptRepo.GetByIdAsync(attemptId);
            if (a == null) throw new KeyNotFoundException($"Attempt {attemptId} not found");
            _attemptRepo.Delete(a);
            await _uow.CommitAsync();
        }

        public async Task<AttemptDto> SaveAttemptAsync(Attempt attempt)
        {
            await _attemptRepo.AddAsync(attempt);
            await _uow.CommitAsync();

            return new AttemptDto
            {
                Id = attempt.Id,
                SubjectId = attempt.SubjectId,
                SubmittedAt = attempt.SubmittedAt,
                Score = attempt.Score,
                MaxScore = attempt.MaxScore
            };
        }
    }
}
