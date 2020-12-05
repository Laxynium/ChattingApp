using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Modules;
using Microsoft.Extensions.Logging;

namespace InstantMessenger.Shared.MessageBrokers
{
    internal sealed class MessageBroker : IMessageBroker
    {
        private readonly IModuleClient _client;
        private readonly ILogger<MessageBroker> _logger;

        public MessageBroker(IModuleClient client, ILogger<MessageBroker> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task PublishAsync(IIntegrationEvent @event)
        {
            _logger.LogTrace($"Publishing message of type {{{@event.GetType().Name}}}.");
            await _client.PublishAsync(@event);
            _logger.LogTrace($"Successfully published message of type {{{@event.GetType().Name}}}.");
        }

        public async Task PublishAsync(IEnumerable<IIntegrationEvent> events)
        {
            var tasks = events.Select(PublishAsync).ToArray();
            await Task.WhenAll(tasks);
        }
    }
}
