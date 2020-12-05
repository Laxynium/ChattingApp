using System.Threading.Tasks;

namespace InstantMessenger.Shared.Decorators.UoW
{
    public interface IUnitOfWork
    {
        Task Commit();
    }
}