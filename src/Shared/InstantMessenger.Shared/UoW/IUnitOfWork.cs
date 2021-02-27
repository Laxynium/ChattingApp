using System.Threading.Tasks;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Shared.UoW
{
    public interface IUnitOfWork<TModule> where TModule : IModule
    {
        Task Commit();
    }
}