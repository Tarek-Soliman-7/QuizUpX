using Domain.Entities;

namespace Domain.Contracts
{
    public interface ISubjectRepository : IRepository<Subject>
    {
        Task<Subject?> GetByNameAsync(string name);
    }
}