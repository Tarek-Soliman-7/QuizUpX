using System.Threading.Tasks;


namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        IStudentRepository Students { get; }
        ISubjectRepository Subjects { get; }
        IQuestionRepository Questions { get; }
        IAttemptRepository Attempts { get; }
        IAttemptAnswerRepository AttemptAnswers { get; }

        Task<int> CompleteAsync();
    }

}