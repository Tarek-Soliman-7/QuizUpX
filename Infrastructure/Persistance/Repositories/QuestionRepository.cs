using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;

        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Question>> GetBySubjectIdAsync(int subjectId)
        {
            return await _context.Questions
                .Where(q => q.SubjectId == subjectId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Question?> GetByIdAsync(int id)
        {
            return await _context.Questions
                .FirstOrDefaultAsync(q => q.Id == id);
        }
    }

}