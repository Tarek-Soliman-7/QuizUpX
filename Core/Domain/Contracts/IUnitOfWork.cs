using System.Threading.Tasks;


namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        ISubjectRepository Subjects { get; }
        IQuestionRepository Questions { get; }
        IAttemptRepository Attempts { get; }
        IStudentRepository Students { get; }
        Task<int> CommitAsync();
    }
}