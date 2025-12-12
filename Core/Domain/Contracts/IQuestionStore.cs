using Domain.Entities;

namespace Domain.Contracts
{
    public interface IQuestionStore
    {
        Task<List<Question>> GetAllAsync();
        Task<List<Question>> GetBySubjectAsync(int subjectId);
        Task<List<Question>> GetRandomBySubjectAsync(int subjectId, int count);
        Task<Question?> GetByIdAsync(int id);

        Task<Question> AddAsync(Question question);
        Task<bool> UpdateAsync(int id, Question question);
        Task<bool> DeleteAsync(int id);

        // optional: force save to disk (Json store will call internally)
        Task SaveAsync();
    }
}
