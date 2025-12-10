namespace Application.Contracts
{
    public interface IQuestionService
    {
        Task<Question> CreateAsync(Question q);
        Task<IEnumerable<Question>> GetBySubjectAsync(int subjectId);
        Task<Question?> GetByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}