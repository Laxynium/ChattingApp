using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Outbox;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Shared.Decorators.UoW
{
    internal sealed class OutboxIntegrationEventsPublisher<TDbContext> : IIntegrationEventsPublisher<TDbContext> where TDbContext:DbContext
    {
        private readonly IMessageOutbox _outbox;

        public OutboxIntegrationEventsPublisher(MessageOutbox<TDbContext> outbox)
        {
            _outbox = outbox;
        }
        public void AddEvents(params IIntegrationEvent[] events)
        {
            foreach (var integrationEvent in events)
            {
                _outbox.AddAsync(integrationEvent);
            }
        }

        public Task PublishAsync()
        {
            //Handled by outbox processor
            return Task.CompletedTask;
        }
    }
}