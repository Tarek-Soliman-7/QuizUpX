using Domain.Contracts;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistance.Data;

namespace Persistance.Repositories
{
    public class AttemptRepository : IAttemptRepository
    {
        private readonly AppDbContext _db;
        public AttemptRepository(AppDbContext db) => _db = db;

        public async Task AddAsync(Attempt attempt)
        {
            await _db.Attempts.AddAsync(attempt);
        }

        public void Delete(Attempt attempt)
        {
            _db.Attempts.Remove(attempt);
        }

        public async Task<int> CountAsync(string? userId = null, int? subjectId = null)
        {
            var q = _db.Attempts.AsQueryable();
            if (!string.IsNullOrEmpty(userId)) q = q.Where(a => a.UserId == userId);
            if (subjectId.HasValue) q = q.Where(a => a.SubjectId == subjectId.Value);
            return await q.CountAsync();
        }

        public async Task<Attempt?> GetByIdAsync(int id)
        {
            return await _db.Attempts.FindAsync(id);
        }

        public async Task<IReadOnlyList<Attempt>> GetBySubjectIdAsync(int subjectId)
        {
            return await _db.Attempts.Where(a => a.SubjectId == subjectId)
                                          .OrderByDescending(a => a.SubmittedAt)
                                          .ToListAsync();
        }

        public async Task<IReadOnlyList<Attempt>> GetByUserIdAsync(string userId, int? take = null, int? skip = null)
        {
            var q = _db.Attempts.Where(a => a.UserId == userId).OrderByDescending(a => a.SubmittedAt).AsQueryable();
            if (skip.HasValue) q = q.Skip(skip.Value);
            if (take.HasValue) q = q.Take(take.Value);
            return await q.ToListAsync();
        }

        public async Task<IReadOnlyList<Attempt>> ListAsync(string? userId = null, int? subjectId = null, int skip = 0, int take = 50)
        {
            var q = _db.Attempts.AsQueryable();

            if (!string.IsNullOrEmpty(userId)) q = q.Where(a => a.UserId == userId);
            if (subjectId.HasValue) q = q.Where(a => a.SubjectId == subjectId.Value);

            q = q.OrderByDescending(a => a.SubmittedAt).Skip(skip).Take(take);
            return await q.ToListAsync();
        }

        public void UpdateAsync(Attempt attempt)
        =>  _db.Attempts.Update(attempt);
    }
}
