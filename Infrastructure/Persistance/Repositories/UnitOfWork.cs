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
        private AttemptRepository? _attemptRepository;
        private AttemptAnswerRepository? _attemptAnswerRepository;
        private StudentRepository? _studentRepository;


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }


        public ISubjectRepository Subjects => _subjectRepository ??= new SubjectRepository(_context);
        public IQuestionRepository Questions => _questionRepository ??= new QuestionRepository(_context);

        public IAttemptRepository Attempts => _attemptRepository ??= new AttemptRepository(_context);

        public IStudentRepository Students => _studentRepository ??= new StudentRepository(_context);

        public IAttemptAnswerRepository AttemptAnswers => _attemptAnswerRepository ??= new AttemptAnswerRepository(_context);

        public Task<int> CompleteAsync() => _context.SaveChangesAsync();
    }
}