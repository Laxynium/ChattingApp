using System.Threading.Tasks;

namespace InstantMessenger.PrivateMessages.Api
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}