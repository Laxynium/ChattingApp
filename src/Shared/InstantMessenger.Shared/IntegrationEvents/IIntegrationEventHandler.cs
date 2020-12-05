using System.Threading.Tasks;

namespace InstantMessenger.Shared.IntegrationEvents
{
    public interface IIntegrationEventHandler<in TEvent> where TEvent : class, IIntegrationEvent
    {
        Task HandleAsync(TEvent @event);
    }
}