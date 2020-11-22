using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Events
{
    internal sealed class EventDispatcher : IEventDispatcher
    {
        private readonly IServiceScopeFactory _factory;

        public EventDispatcher(IServiceScopeFactory factory)
        {
            _factory = factory;
        }
        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            using var scope = _factory.CreateScope();
            var handlers = scope.ServiceProvider.GetServices<IEventHandler<TEvent>>();
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}