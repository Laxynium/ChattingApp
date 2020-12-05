using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Infrastructure.Database;
using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Decorators.UoW;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.PrivateMessages.Infrastructure.Decorators
{
    [Decorator]
    public class PublishDomainEventsEventHandlerDecorator<TEvent> : IDomainEventHandler<TEvent>
        where TEvent : class, IDomainEvent
    {
        private readonly IDomainEventHandler<TEvent> _innerHandler;
        private readonly DomainEventPublisher<PrivateMessagesContext> _publisher;

        public PublishDomainEventsEventHandlerDecorator(IDomainEventHandler<TEvent> innerHandler, DomainEventPublisher<PrivateMessagesContext> publisher)
        {
            _innerHandler = innerHandler;
            _publisher = publisher;
        }
        public async Task HandleAsync(TEvent @event)
        {
            await _innerHandler.HandleAsync(@event);
            await _publisher.Publish();
        }
    }
}