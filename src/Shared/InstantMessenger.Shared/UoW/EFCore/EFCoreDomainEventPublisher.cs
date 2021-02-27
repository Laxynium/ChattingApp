using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Messages.Events;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Shared.UoW.EFCore
{
    internal sealed class EFCoreDomainEventPublisher<TModule> : IDomainEventPublisher<TModule> where TModule : IModule
    {
        private readonly IDomainEventAccessor<TModule> _domainEventAccessor;
        private readonly IEnumerable<IDomainEventMapper> _mappers;
        private readonly IDomainEventDispatcher _dispatcher;
        private readonly IIntegrationEventsPublisher<TModule> _integrationEventsPublisher;

        public EFCoreDomainEventPublisher(IDomainEventAccessor<TModule> domainEventAccessor,
            IEnumerable<IDomainEventMapper> mappers, IDomainEventDispatcher dispatcher,
            IIntegrationEventsPublisher<TModule> integrationEventsPublisher)
        {
            _domainEventAccessor = domainEventAccessor;
            _mappers = mappers;
            _dispatcher = dispatcher;
            _integrationEventsPublisher = integrationEventsPublisher;
        }

        public async Task Publish()
        {
            var domainEvents = _domainEventAccessor.Events.ToList();
            _domainEventAccessor.ClearAllDomainEvents();

            var integrationEvents = _mappers.SelectMany(x => x.Map(domainEvents)).ToArray();

            await _integrationEventsPublisher.AddEvents(integrationEvents);

            var tasks = domainEvents.Select(e => _dispatcher.PublishAsync(e));
            await Task.WhenAll(tasks);
        }
    }
}