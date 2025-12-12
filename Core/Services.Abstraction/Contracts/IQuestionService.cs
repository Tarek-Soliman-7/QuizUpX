using Domain.Entities;
using Shared.Dtos;

namespace Application.Contracts
{
    public interface IQuestionService
    {
        Task<QuestionDto> CreateAsync(QuestionDto q);
        Task<IEnumerable<Question>> GetAllAsync();
        Task<IEnumerable<Question>> GetBySubjectAsync(int subjectId);
        Task<Question?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
        Task<QuestionDto> Update(int id,QuestionDto q);


    }
}