using System.Threading.Tasks;

namespace InstantMessenger.Profiles
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}