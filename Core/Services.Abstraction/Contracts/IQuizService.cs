using Shared.Dtos;

namespace Services.Abstraction.Contracts
{
    public interface IQuizService
    {
        Task<StudentLoginResponseDto> LoginAsync(StudentLoginRequestDto dto);
        Task<List<QuestionDto>> GetQuestionsAsync(int subjectId);
        Task<SubmitResultDto> SubmitQuizAsync(SubmitQuizDto dto);
        Task<List<QuestionResultDto>> GetReviewAsync(int attemptId);
    }

}
