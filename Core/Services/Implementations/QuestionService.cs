using Application.Contracts;
using Domain.Contracts;
using Domain.Entities;
using Shared.Dtos;

namespace Core.Services
{
    public class QuestionService :  IQuestionService
    {
        private readonly IUnitOfWork _uow;


        public QuestionService(IUnitOfWork uow)
        {
            _uow = uow;
        }


        public async Task<QuestionDto> CreateAsync(QuestionDto q)
        {
            // Basic validation
            var subject = await _uow.Subjects.GetByIdAsync(q.subjectId);
            if (subject == null)
                throw new KeyNotFoundException($"Subject with id {q.subjectId} not found.");
            var qustion = new Question()
            {
                id = q.id,
                choices = q.choices,
                correctIndex = q.correctIndex,
                mark = q.mark,
                questionType = q.questionType,
                Subject = subject,
                subjectId = q.subjectId,
                title = q.title
            };

            await _uow.Questions.AddAsync(qustion);
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

        public async Task<IEnumerable<Question>> GetAllAsync()
        => await _uow.Questions.GetAllAsync();
        

        public Task<Question?> GetByIdAsync(int id)
        => _uow.Questions.GetByIdAsync(id);


        public Task<IEnumerable<Question>> GetBySubjectAsync(int subjectId)
        => _uow.Questions.GetBySubjectAsync(subjectId);

        public async Task<QuestionDto> Update(int id, QuestionDto q)
        {
            var subject = await _uow.Subjects.GetByIdAsync(q.subjectId);
            if (subject == null)
                throw new KeyNotFoundException($"Subject with id {q.subjectId} not found.");
            var qustion = new Question()
            {
                id = q.id,
                choices = q.choices,
                correctIndex = q.correctIndex,
                mark = q.mark,
                questionType = q.questionType,
                Subject = subject,
                subjectId = q.subjectId,
                title = q.title
            };

             _uow.Questions.Update(qustion);
            await _uow.CommitAsync();
            return q;
        }
    }
}