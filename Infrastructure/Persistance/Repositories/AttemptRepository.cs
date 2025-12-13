using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class AttemptRepository : IAttemptRepository
    {
        private readonly AppDbContext _context;

        public AttemptRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Attempt> AddAsync(Attempt attempt)
        {
            await _context.Attempts.AddAsync(attempt);
            return attempt;
        }

        public async Task<Attempt?> GetByStudentAndSubjectAsync(int studentId, int subjectId)
        {
            return await _context.Attempts
                .FirstOrDefaultAsync(a =>
                    a.StudentId == studentId &&
                    a.SubjectId == subjectId);
        }

        public async Task<Attempt?> GetByIdWithAnswersAsync(int attemptId)
        {
            return await _context.Attempts
                .Include(a => a.Answers)
                    .ThenInclude(aa => aa.Question)
                .FirstOrDefaultAsync(a => a.Id == attemptId);
        }
    }

}
