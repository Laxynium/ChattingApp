using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Messages.Events;
using InstantMessenger.Shared.UoW;

namespace InstantMessenger.Identity.Infrastructure.Decorators
{
    [Decorator]
    internal sealed class
        PublishDomainEventsEventHandlerDecorator<TEvent> : PublishDomainEventsEventHandlerDecorator<IdentityModule,
            TEvent>
        where TEvent : class, IDomainEvent
    {
        public PublishDomainEventsEventHandlerDecorator(IDomainEventHandler<TEvent> domainEventHandler,
            IDomainEventPublisher<IdentityModule> domainEventPublisher) : base(domainEventHandler, domainEventPublisher)
        {
        }
    }
}