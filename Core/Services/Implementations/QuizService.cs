using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
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
                .GetByUniversityCodeAsync(dto.universityCode);

            if (student == null || student.Pin != dto.pin)
            {
                return new StudentLoginResponseDto
                {
                    success = false,
                    message = "Invalid code or pin"
                };
            }

            return new StudentLoginResponseDto
            {
                success = true,
                studentId = student.Id,
                studentName = student.Name,
                message = "Login successful"
            };
        }

        public async Task<List<QuestionDto>> GetQuestionsAsync(int subjectId)
        {
            var questions = await _questionRepo
                .GetBySubjectIdAsync(subjectId);

            return questions.Select(q => new QuestionDto
            {
                questionId = q.Id,
                title = q.Title,
                choices = q.Choices,
                correctIndex = q.CorrectIndex, // مؤقتًا
                mark = q.Mark
            }).ToList();
        }

        public async Task<SubmitResultDto> SubmitQuizAsync(SubmitQuizDto dto)
        {
            var attempt = await _attemptRepo
                .GetByStudentAndSubjectAsync(dto.studentId, dto.subjectId);

            if (attempt == null)
            {
                attempt = new Attempt
                {
                    StudentId = dto.studentId,
                    SubjectId = dto.subjectId,
                    StartedAt = DateTime.UtcNow
                };

                await _attemptRepo.AddAsync(attempt);
                await _unitOfWork.CompleteAsync();
            }

            int totalScore = 0;
            int correctAnswers = 0;

            foreach (var ans in dto.answers)
            {
                var question = await _questionRepo.GetByIdAsync(ans.questionId);
                if (question == null) continue;

                bool isCorrect = ans.selectedIndex == question.CorrectIndex;

                if (isCorrect)
                {
                    totalScore += question.Mark;
                    correctAnswers++;
                }

                var attemptAnswer = new AttemptAnswer
                {
                    AttemptId = attempt.Id,
                    QuestionId = question.Id,
                    SelectedIndex = ans.selectedIndex,
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
                totalScore = totalScore,
                correctAnswers = correctAnswers,
                incorrectAnswers = dto.answers.Count - correctAnswers
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

        public async Task<List<SubjectDto>> GetAllSubjectAsync()
        {
            var subjects = await _unitOfWork.Subjects.GetAllAsync();

            return subjects.Select(s => new SubjectDto
            {
                id = s.Id,
                name = s.Name,
                description = s.Description
            }).ToList();
        }
    }
}
