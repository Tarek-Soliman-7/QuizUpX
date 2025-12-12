using Domain.Entities;
using Shared.Dtos;

namespace Services.Abstraction.Contracts
{
    public interface IAttemptService
    {
        Task<AttemptDto> GetAttemptSummaryAsync(int attemptId);
        Task<AttemptDetailsDto> GetAttemptDetailsAsync(int attemptId);
        Task<IReadOnlyList<AttemptDto>> GetUserAttemptsAsync(string userId, int skip = 0, int take = 50);
        Task DeleteAttemptAsync(int attemptId);
        
        Task<AttemptDto> SaveAttemptAsync(Attempt attempt);

    }
}
