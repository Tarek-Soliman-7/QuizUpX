using Domain.Entities;

namespace Domain.Contracts
{
    public interface IQuestionRepository : IRepository<Question>
    {
        Task<IEnumerable<Question>> GetBySubjectAsync(int subjectId);

    }
}