using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class AttemptAnswerRepository : IAttemptAnswerRepository
    {
        private readonly AppDbContext _context;

        public AttemptAnswerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AttemptAnswer answer)
        {
            await _context.AttemptAnswers.AddAsync(answer);
        }

        public async Task<List<AttemptAnswer>> GetByAttemptIdAsync(int attemptId)
        {
            return await _context.AttemptAnswers
                .Include(a => a.Question)
                .Where(a => a.AttemptId == attemptId)
                .AsNoTracking()
                .ToListAsync();
        }
    }

}
