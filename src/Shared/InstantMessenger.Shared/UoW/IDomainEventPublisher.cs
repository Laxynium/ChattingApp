using System.Threading.Tasks;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Shared.UoW
{
    public interface IDomainEventPublisher<TModule> where TModule : IModule
    {
        Task Publish();
    }
}