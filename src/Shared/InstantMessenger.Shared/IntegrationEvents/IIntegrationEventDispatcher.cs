using System.Threading.Tasks;

namespace InstantMessenger.Shared.IntegrationEvents
{
    public interface IIntegrationEventDispatcher
    {
        public Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IIntegrationEvent;
    }
}