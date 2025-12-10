using System.Threading.Tasks;


namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        ISubjectRepository Subjects { get; }
        IQuestionRepository Questions { get; }
        Task<int> CommitAsync();
    }
}