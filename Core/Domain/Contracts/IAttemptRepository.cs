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
        Task<Attempt> AddAsync(Attempt attempt);
        Task<Attempt?> GetByStudentAndSubjectAsync(int studentId, int subjectId);
        Task<Attempt?> GetByIdWithAnswersAsync(int attemptId);
    }

}
