using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IAttemptRepository
    {
        Task<Attempt?> GetByIdAsync(int id);
        Task<IReadOnlyList<Attempt>> GetByUserIdAsync(string userId, int? take = null, int? skip = null);
        Task<IReadOnlyList<Attempt>> GetBySubjectIdAsync(int subjectId);
        Task AddAsync(Attempt attempt);
        void Delete(Attempt attempt);
        void UpdateAsync(Attempt attempt);
        Task<int> CountAsync(string? userId = null, int? subjectId = null);
        Task<IReadOnlyList<Attempt>> ListAsync(string? userId = null, int? subjectId = null, int skip = 0, int take = 50);
    }
}
