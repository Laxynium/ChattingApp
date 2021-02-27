using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Messages.Events;
using InstantMessenger.Shared.UoW;

namespace InstantMessenger.PrivateMessages.Infrastructure.Decorators
{
    [Decorator]
    internal sealed class
        PublishDomainEventsEventHandlerDecorator<TEvent> : PublishDomainEventsEventHandlerDecorator<
            PrivateMessagesModule,
            TEvent>
        where TEvent : class, IDomainEvent
    {
        public PublishDomainEventsEventHandlerDecorator(IDomainEventHandler<TEvent> domainEventHandler,
            IDomainEventPublisher<PrivateMessagesModule> domainEventPublisher) : base(domainEventHandler,
            domainEventPublisher)
        {
        }
    }
}