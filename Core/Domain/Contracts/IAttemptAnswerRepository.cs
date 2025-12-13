using Domain.Entities;

namespace Domain.Contracts
{
    public interface IAttemptAnswerRepository
    {
        Task AddAsync(AttemptAnswer answer);
        Task<List<AttemptAnswer>> GetByAttemptIdAsync(int attemptId);
    }
}
