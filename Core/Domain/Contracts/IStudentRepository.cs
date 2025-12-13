using Domain.Entities.IdentityModule;

namespace Domain.Contracts
{
    public interface IStudentRepository
    {
        Task<Student?> GetByUniversityCodeAsync(string code);
        Task<Student?> GetByIdAsync(int id);
    }

}
