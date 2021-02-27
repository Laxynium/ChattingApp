using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Messages.Events;
using InstantMessenger.Shared.UoW;

namespace InstantMessenger.Friendships.Infrastructure.Decorators
{
    [Decorator]
    internal sealed class
        PublishDomainEventsEventHandlerDecorator<TEvent> : PublishDomainEventsEventHandlerDecorator<FriendshipsModule,
            TEvent
        >
        where TEvent : class, IDomainEvent
    {
        public PublishDomainEventsEventHandlerDecorator(IDomainEventHandler<TEvent> domainEventHandler,
            IDomainEventPublisher<FriendshipsModule> domainEventPublisher) : base(domainEventHandler,
            domainEventPublisher)
        {
        }
    }
}