using Domain.Entities;

namespace Domain.Contracts
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetBySubjectIdAsync(int subjectId);
        Task<Question?> GetByIdAsync(int id);
    }

}