using System.Threading.Tasks;

namespace InstantMessenger.Groups.Api
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}