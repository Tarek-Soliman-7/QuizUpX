using Shared.Dtos;

namespace Services.Abstraction.Contracts
{
    public interface IExamService
    {
        Task<SubmitResultDto> GradeAndSaveAttemptAsync(SubmitDto dto, string? userId = null, string? ipAddress = null, string? deviceInfo = null);
        Task<StartQuizResponse> StartExamAsync(StartQuizRequest req);
        Task<SubmitResultDto> SubmitAttemptAsync(SubmitResultDto req, string? userId = null);
        Task<SubmitResultDto?> GetAttemptResultAsync(int attemptId, bool includeCorrectAnswers = false);
    }
}
