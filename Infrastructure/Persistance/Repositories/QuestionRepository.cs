using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(AppDbContext context) : base(context) { }


        public async Task<IEnumerable<Question>> GetBySubjectAsync(int subjectId)
        {
            return await _dbSet
            .Where(q => q.subjectId == subjectId)
            .AsNoTracking()
            .ToListAsync();
        }
    }
}