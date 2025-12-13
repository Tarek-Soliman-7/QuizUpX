using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext _context;

        public StudentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Student?> GetByUniversityCodeAsync(string code)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.UniversityCode == code);
        }

        public async Task<Student?> GetByIdAsync(int id)
        {
            return await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }

}
