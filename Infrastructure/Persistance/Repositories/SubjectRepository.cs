using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class SubjectRepository : Repository<Subject>, ISubjectRepository
    {
        public SubjectRepository(AppDbContext context) : base(context) { }


        public async Task<Subject?> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(s => s.name == name);
        }
    }
}