using System.Threading.Tasks;

namespace InstantMessenger.Friendships.Api
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}