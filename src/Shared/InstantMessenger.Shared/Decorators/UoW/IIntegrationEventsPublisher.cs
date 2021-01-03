using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;

namespace InstantMessenger.Shared.Decorators.UoW
{
    public interface IIntegrationEventsPublisher<TDbContext>
    {
        void AddEvents(params IIntegrationEvent[] events);
        Task PublishAsync();
    }
}