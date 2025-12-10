using Application.Contracts;
using Domain.Contracts;

namespace Core.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _uow;


        public QuestionService(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<Question> CreateAsync(Question q)
        {
            // Basic validation
            var subject = await _uow.Subjects.GetByIdAsync(q.subjectId);
            if (subject == null)
                throw new KeyNotFoundException($"Subject with id {q.subjectId} not found.");


            await _uow.Questions.AddAsync(q);
            await _uow.CommitAsync();
            return q;
        }


        public Task DeleteAsync(int id)
        {
            return Task.Run(async () =>
            {
                var entity = await _uow.Questions.GetByIdAsync(id);
                if (entity != null)
                {
                    _uow.Questions.Remove(entity);
                    await _uow.CommitAsync();
                }
            });
        }


        public Task<Question?> GetByIdAsync(int id)
        => _uow.Questions.GetByIdAsync(id);


        public Task<IEnumerable<Question>> GetBySubjectAsync(int subjectId)
        => _uow.Questions.GetBySubjectAsync(subjectId);
    }
}