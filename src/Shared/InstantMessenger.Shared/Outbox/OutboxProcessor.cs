using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.MessageBrokers;
using InstantMessenger.Shared.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InstantMessenger.Shared.Outbox
{
    internal sealed class OutboxProcessor<TModule> : BackgroundService where TModule : IModule
    {
        private static readonly JsonSerializerSettings SerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<OutboxProcessor<TModule>> _logger;
        private readonly IServiceProvider _scopeFactory;
        private readonly OutboxOptions _options;
        private readonly TimeSpan _interval;

        public OutboxProcessor(OutboxOptions options, IMessageBroker messageBroker, IServiceProvider scopeFactory,
            ILogger<OutboxProcessor<TModule>> logger)
        {
            _messageBroker = messageBroker;
            _logger = logger;
            _scopeFactory = scopeFactory;
            _options = options;
            _interval = TimeSpan.FromMilliseconds(options.IntervalMilliseconds);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_options.Enabled)
            {
                _logger.LogInformation("Outbox is disabled");
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var messageAccessor = scope.ServiceProvider.GetRequiredService<IOutboxMessageAccessor<TModule>>();
            ;
            _logger.LogInformation("Outbox is enabled");
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogTrace($"Started processing outbox messages...");
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    await SendOutboxMessageAsync(messageAccessor);
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"There was an error when processing outbox.");
                    _logger.LogError(e, e.Message);
                }

                stopwatch.Stop();
                _logger.LogTrace($"Finished processing outbox messages in {stopwatch.ElapsedMilliseconds} ms.");


                await Task.Delay(_interval, stoppingToken);
            }
        }

        public async Task SendOutboxMessageAsync(IOutboxMessageAccessor<TModule> messageAccessor)
        {
            _logger.LogTrace($"Started processing outbox message [context: {typeof(TModule).Name}]...");
            var messages = await messageAccessor.GetUnsent();

            if (!messages.Any())
            {
                _logger.LogTrace($"No messages to be processed in outbox [context: {typeof(TModule).Name}].");
                return;
            }

            _logger.LogTrace($"Found {messages.Count} unsent messages in outbox [context: {typeof(TModule).Name}].");

            foreach (var message in messages.OrderBy(m => m.OccurredOn))
            {
                var @event = JsonConvert.DeserializeObject(message.Data, SerializerSettings);
                await _messageBroker.PublishAsync((IIntegrationEvent) @event);

                await messageAccessor.MarkAsProcessed(message);
            }
        }
    }

    internal sealed class CleanUpOutboxProcessor<TModule> : IHostedService where TModule : IModule
    {
        private readonly ILogger<CleanUpOutboxProcessor<TModule>> _logger;
        private readonly IServiceProvider _scopeFactory;
        private readonly OutboxOptions _options;
        private Timer _cleanExpiredMessagesTimer;
        private readonly TimeSpan _interval;

        public CleanUpOutboxProcessor(OutboxOptions options, IServiceProvider scopeFactory,
            ILogger<CleanUpOutboxProcessor<TModule>> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _options = options;
            _interval = TimeSpan.FromMilliseconds(options.IntervalMilliseconds);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!_options.Enabled)
                return Task.CompletedTask;

            if (_options.CleanupIntervalHours > 0)
            {
                _cleanExpiredMessagesTimer = new Timer(CleanOutdatedMessages, null, TimeSpan.Zero,
                    TimeSpan.FromHours(_options.CleanupIntervalHours));
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (!_options.Enabled)
                return Task.CompletedTask;

            _cleanExpiredMessagesTimer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
            ;
        }

        public void CleanOutdatedMessages(object state)
        {
            _ = CleanExpiredMessagesAsync();
        }

        public async Task CleanExpiredMessagesAsync()
        {
            var jobId = Guid.NewGuid().ToString("N");
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var messageAccessor = scope.ServiceProvider.GetRequiredService<IOutboxMessageAccessor<TModule>>();
                _logger.LogTrace($"Started processing expired outbox messages ... [job id: '{jobId}']");
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var expiredMessages = await messageAccessor.GetExpired();
                _logger.LogTrace($"Found {expiredMessages.Count} expired messages in outbox [job id: '{jobId}'].");

                if (!expiredMessages.Any())
                {
                    _logger.LogTrace($"No expired messages to be cleaned in outbox [job id: '{jobId}'].");
                    return;
                }

                await messageAccessor.ClearExpired(expiredMessages);

                stopwatch.Stop();
                _logger.LogTrace(
                    $"Processed {expiredMessages.Count} expired outbox messages in {stopwatch.ElapsedMilliseconds} ms [job id: '{jobId}'].");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Processing outbox message [job id: '{jobId}'] failed.");
            }
        }
    }
}