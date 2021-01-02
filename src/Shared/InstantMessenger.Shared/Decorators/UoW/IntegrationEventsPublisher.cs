using System.Collections.Generic;
using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.MessageBrokers;

namespace InstantMessenger.Shared.Decorators.UoW
{
    public class IntegrationEventsPublisher : IIntegrationEventsPublisher
    {
        private readonly IMessageBroker _messageBroker;
        private readonly List<IIntegrationEvent> _events = new List<IIntegrationEvent>();

        public IntegrationEventsPublisher(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }
        public void AddEvents(params IIntegrationEvent[] events)
        {
            _events.AddRange(events);
        }

        public async Task PublishAsync()
        {
            await _messageBroker.PublishAsync(_events.ToArray());
            _events.Clear();
        }
    }
}