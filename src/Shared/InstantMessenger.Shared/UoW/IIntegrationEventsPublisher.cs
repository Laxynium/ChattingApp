using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Shared.UoW
{
    public interface IIntegrationEventsPublisher<TTModule> where TTModule : IModule
    {
        Task AddEvents(params IIntegrationEvent[] events);
        Task PublishAsync();
    }
}