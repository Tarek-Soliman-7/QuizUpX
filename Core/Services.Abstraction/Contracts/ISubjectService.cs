using Domain.Entities;

namespace Application.Contracts
{
    public interface ISubjectService
    {
        Task<Subject> CreateAsync(Subject s);
        Task<IEnumerable<Subject>> GetAllAsync();
        Task<Subject?> GetByIdAsync(int id);
    }
}