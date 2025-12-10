using Application.Contracts;
using Domain.Contracts;
using Domain.Entities;

namespace Services.Implementations
{
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _uow;


        public SubjectService(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<Subject> CreateAsync(Subject s)
        {
            var exists = await _uow.Subjects.GetByNameAsync(s.name);
            if (exists != null)
                return exists; // or throw if you prefer


            await _uow.Subjects.AddAsync(s);
            await _uow.CommitAsync();
            return s;
        }


        public Task<IEnumerable<Subject>> GetAllAsync()
        => _uow.Subjects.GetAllAsync();


        public Task<Subject?> GetByIdAsync(int id)
        => _uow.Subjects.GetByIdAsync(id);
    }
}
