using System.Collections.Generic;
using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.MessageBrokers;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Shared.UoW
{
    internal sealed class InMemoryIntegrationEventsPublisher<TModule> : IIntegrationEventsPublisher<TModule>
        where TModule : IModule
    {
        private readonly IMessageBroker _messageBroker;
        private readonly List<IIntegrationEvent> _events = new();

        public InMemoryIntegrationEventsPublisher(IMessageBroker messageBroker)
        {
            _messageBroker = messageBroker;
        }

        public Task AddEvents(params IIntegrationEvent[] events)
        {
            _events.AddRange(events);
            return Task.CompletedTask;
        }

        public async Task PublishAsync()
        {
            await _messageBroker.PublishAsync(_events.ToArray());
            _events.Clear();
        }
    }
}