using System.Collections.Generic;
using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;

namespace InstantMessenger.Shared.MessageBrokers
{
    public interface IMessageBroker
    {
        Task PublishAsync(IIntegrationEvent @event);
        Task PublishAsync(IEnumerable<IIntegrationEvent> events);
    }
}
