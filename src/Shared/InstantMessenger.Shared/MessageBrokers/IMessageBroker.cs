using System.Collections.Generic;
using System.Threading.Tasks;
using InstantMessenger.Shared.Events;

namespace InstantMessenger.Shared.MessageBrokers
{
    public interface IMessageBroker
    {
        Task PublishAsync(IEvent @event);
        Task PublishAsync(IEnumerable<IEvent> events);
    }
}
