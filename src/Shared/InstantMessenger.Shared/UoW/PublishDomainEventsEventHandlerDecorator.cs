using System.Threading.Tasks;
using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Messages.Events;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Shared.UoW
{
    [Decorator]
    public  abstract class PublishDomainEventsEventHandlerDecorator<TModule, TEvent> : IDomainEventHandler<TEvent>
        where TModule : IModule
        where TEvent : class, IDomainEvent
    {
        private readonly IDomainEventHandler<TEvent> _domainEventHandler;
        private readonly IDomainEventPublisher<TModule> _domainEventPublisher;

        public PublishDomainEventsEventHandlerDecorator(IDomainEventHandler<TEvent> domainEventHandler,
            IDomainEventPublisher<TModule> domainEventPublisher)
        {
            _domainEventHandler = domainEventHandler;
            _domainEventPublisher = domainEventPublisher;
        }

        public async Task HandleAsync(TEvent @event)
        {
            await _domainEventHandler.HandleAsync(@event);
            await _domainEventPublisher.Publish();
        }
    }
}