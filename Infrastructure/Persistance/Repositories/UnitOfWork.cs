using Domain.Contracts;
using Persistance.Data;
using Persistance.Repositories;

namespace Persistance.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private SubjectRepository? _subjectRepository;
        private QuestionRepository? _questionRepository;


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }


        public ISubjectRepository Subjects => _subjectRepository ??= new SubjectRepository(_context);
        public IQuestionRepository Questions => _questionRepository ??= new QuestionRepository(_context);


        public Task<int> CommitAsync() => _context.SaveChangesAsync();
    }
}