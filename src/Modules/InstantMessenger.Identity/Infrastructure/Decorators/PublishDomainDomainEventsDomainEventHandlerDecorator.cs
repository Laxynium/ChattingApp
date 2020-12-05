using System.Threading.Tasks;
using InstantMessenger.Identity.Infrastructure.Database;
using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Decorators.UoW;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Identity.Infrastructure.Decorators
{
    [Decorator]
    public class PublishDomainEventsEventHandlerDecorator<TEvent> : IDomainEventHandler<TEvent>
        where TEvent : class, IDomainEvent
    {
        private readonly IDomainEventHandler<TEvent> _innerHandler;
        private readonly DomainEventPublisher<IdentityContext> _publisher;

        public PublishDomainEventsEventHandlerDecorator(IDomainEventHandler<TEvent> innerHandler, DomainEventPublisher<IdentityContext> publisher)
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