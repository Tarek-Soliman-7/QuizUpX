using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _db;

        public StudentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Student?> GetByUniversityCodeAsync(string code)
        {
            return await _db.Students
                .FirstOrDefaultAsync(s => s.UniversityCode == code);
        }

        public async Task AddAsync(Student student)
        {
            await _db.Students.AddAsync(student);
        }

        public Task UpdateAsync(Student student)
        {
            _db.Students.Update(student);
            return Task.CompletedTask;
        }
    }
}
