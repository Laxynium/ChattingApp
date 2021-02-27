using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Messages.Events;
using InstantMessenger.Shared.UoW;

namespace InstantMessenger.Groups.Infrastructure.Decorators
{
    [Decorator]
    internal sealed class
        PublishDomainEventsEventHandlerDecorator<TEvent> : PublishDomainEventsEventHandlerDecorator<GroupsModule, TEvent
        >
        where TEvent : class, IDomainEvent
    {
        public PublishDomainEventsEventHandlerDecorator(IDomainEventHandler<TEvent> domainEventHandler,
            IDomainEventPublisher<GroupsModule> domainEventPublisher) : base(domainEventHandler, domainEventPublisher)
        {
        }
    }
}