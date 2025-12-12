using Domain.Entities.IdentityModule;

namespace Domain.Contracts
{
    public interface IStudentRepository
    {
        Task<Student?> GetByUniversityCodeAsync(string code);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
    }
}
