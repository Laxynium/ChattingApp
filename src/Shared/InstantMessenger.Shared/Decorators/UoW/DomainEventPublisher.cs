using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Messages.Events;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Shared.Decorators.UoW
{
    public class DomainEventPublisher<TDbContext> where TDbContext : DbContext
    {
        private readonly DomainEventsAccessor<TDbContext> _accessor;
        private readonly IIntegrationEventsPublisher _integrationEventsPublisher;
        private readonly IEnumerable<IDomainEventMapper> _mappers;
        private readonly IDomainEventDispatcher _dispatcher;

        public DomainEventPublisher(DomainEventsAccessor<TDbContext> accessor, IIntegrationEventsPublisher integrationEventsPublisher,
            IEnumerable<IDomainEventMapper> mappers,
            IDomainEventDispatcher dispatcher)
        {
            _accessor = accessor;
            _integrationEventsPublisher = integrationEventsPublisher;
            _mappers = mappers;
            _dispatcher = dispatcher;
        }
        public async Task Publish()
        {
            var domainEvents = _accessor.Events.ToList();
            _accessor.ClearAllDomainEvents();

            var integrationEvents = _mappers.SelectMany(x => x.Map(domainEvents)).ToArray();

            _integrationEventsPublisher.AddEvents(integrationEvents);

            var tasks = domainEvents.Select(e => _dispatcher.PublishAsync(e));
            await Task.WhenAll(tasks);
        }
    }
}