using System.Threading.Tasks;

namespace InstantMessenger.Identity.Api
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}