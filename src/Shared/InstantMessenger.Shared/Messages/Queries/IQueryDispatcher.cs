using System.Threading.Tasks;

namespace InstantMessenger.Shared.Messages.Queries
{
    public interface IQueryDispatcher
    {
        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query);
    }
}