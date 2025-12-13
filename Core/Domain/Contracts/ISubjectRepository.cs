using Domain.Entities;

namespace Domain.Contracts
{
    public interface ISubjectRepository
    {
        Task<List<Subject>> GetAllAsync();
        Task<Subject?> GetByIdAsync(int id);
    }

}