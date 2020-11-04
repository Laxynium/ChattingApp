using System.Threading.Tasks;

namespace InstantMessenger.Profiles.Api
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}