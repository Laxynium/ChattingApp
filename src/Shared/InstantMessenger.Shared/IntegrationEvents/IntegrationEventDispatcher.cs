using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.IntegrationEvents
{
    internal sealed class IntegrationEventDispatcher : IIntegrationEventDispatcher
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public IntegrationEventDispatcher(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IIntegrationEvent
        {
            using var scope = _scopeFactory.CreateScope();
            var handlers = scope.ServiceProvider.GetServices<IIntegrationEventHandler<TEvent>>();
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}