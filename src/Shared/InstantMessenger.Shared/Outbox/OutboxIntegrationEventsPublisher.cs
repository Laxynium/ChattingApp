using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Modules;
using InstantMessenger.Shared.UoW;

namespace InstantMessenger.Shared.Outbox
{
    internal sealed class OutboxIntegrationEventsPublisher<TModule> : IIntegrationEventsPublisher<TModule>
        where TModule : IModule
    {
        private readonly IMessageOutbox<TModule> _outbox;

        public OutboxIntegrationEventsPublisher(IMessageOutbox<TModule> outbox)
        {
            _outbox = outbox;
        }

        public async Task AddEvents(params IIntegrationEvent[] events)
        {
            foreach (var integrationEvent in events)
            {
                await _outbox.AddAsync(integrationEvent);
            }
        }

        public Task PublishAsync()
        {
            //Handled by outbox processor
            return Task.CompletedTask;
        }
    }
}