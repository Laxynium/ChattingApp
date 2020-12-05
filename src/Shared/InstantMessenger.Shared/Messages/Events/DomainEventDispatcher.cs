using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Messages.Events
{
    internal sealed class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IServiceProvider _provider;

        public DomainEventDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IDomainEvent
        {
            var handlers = _provider.GetServices<IDomainEventHandler<TEvent>>();
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }

        public async Task PublishAsync(IDomainEvent @event)
        {
            var eventType = @event.GetType();
            var eventHandlerType = typeof(IDomainEventHandler<>).MakeGenericType(eventType);
            var handlers = _provider.GetServices(eventHandlerType);
            foreach (var handler in handlers)
            {
                await (Task) handler.GetType()
                    .GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync))
                    .Invoke(handler, new object[] {@event});
            }
        }
    }
}