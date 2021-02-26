using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.MessageBrokers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InstantMessenger.Shared.Outbox
{
    public class OutboxProcessor<TDbContext> : BackgroundService where TDbContext : DbContext
    {
        private static readonly JsonSerializerSettings SerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<OutboxProcessor<TDbContext>> _logger;
        private readonly IServiceProvider _scopeFactory;
        private readonly OutboxOptions _options;
        private readonly TimeSpan _interval;
        public OutboxProcessor(OutboxOptions options, IMessageBroker messageBroker, IServiceProvider scopeFactory, ILogger<OutboxProcessor<TDbContext>> logger)
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

            _logger.LogInformation("Outbox is enabled");
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogTrace($"Started processing outbox messages...");
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    await SendOutboxMessageAsync();
                }catch (Exception e)
                {
                    _logger.LogError(e,$"There was an error when processing outbox.");
                    _logger.LogError(e, e.Message);
                }

                stopwatch.Stop();
                _logger.LogTrace($"Finished processing outbox messages in {stopwatch.ElapsedMilliseconds} ms.");


                await Task.Delay(_interval, stoppingToken);
            }
        }

        public async Task SendOutboxMessageAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
            var contextName = dbContext.GetType().Name;
            _logger.LogTrace($"Started processing outbox message [context: {contextName}]...");
            var outboxMessageSet = dbContext.Set<OutboxMessage>();
            var messages = await GetUnsent(outboxMessageSet);

            if (!messages.Any())
            {
                _logger.LogTrace($"No messages to be processed in outbox [context: {contextName}].");
                return;
            }

            _logger.LogTrace($"Found {messages.Count} unsent messages in outbox [context: {contextName}].");

            foreach (var message in messages.OrderBy(m => m.OccurredOn))
            {
                var @event = JsonConvert.DeserializeObject(message.Data, SerializerSettings);
                await _messageBroker.PublishAsync((IIntegrationEvent) @event);

                message.ProcessedDate = DateTime.UtcNow;
                outboxMessageSet.Update(message);
                await dbContext.SaveChangesAsync();
            }
        }

        private async Task<IList<OutboxMessage>> GetUnsent(DbSet<OutboxMessage> outboxMessageSet)
        {
            var messages = await outboxMessageSet
                .Where(x=>x.ProcessedDate == null)
                .ToListAsync();
            return messages;
        }
    }

    public class CleanUpOutboxProcessor<TDbContext> : IHostedService where TDbContext : DbContext
    {

        private readonly ILogger<CleanUpOutboxProcessor<TDbContext>> _logger;
        private readonly IServiceProvider _scopeFactory;
        private readonly OutboxOptions _options;
        private Timer _cleanExpiredMessagesTimer;
        private readonly TimeSpan _interval;
        public CleanUpOutboxProcessor(OutboxOptions options, IServiceProvider scopeFactory, ILogger<CleanUpOutboxProcessor<TDbContext>> logger)
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
                _cleanExpiredMessagesTimer = new Timer(CleanOutdatedMessages, null, TimeSpan.Zero, TimeSpan.FromHours(_options.CleanupIntervalHours));
            }
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            if (!_options.Enabled)
                return Task.CompletedTask;

            _cleanExpiredMessagesTimer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;;
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
                var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
                _logger.LogTrace($"Started processing expired outbox messages ... [job id: '{jobId}']");
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var outboxMessageSet = dbContext.Set<OutboxMessage>();
               
                var expiredMessages = await GetExpired(outboxMessageSet);
                _logger.LogTrace($"Found {expiredMessages.Count} expired messages in outbox [job id: '{jobId}'].");

                if (!expiredMessages.Any())
                {
                    _logger.LogTrace($"No expired messages to be cleaned in outbox [job id: '{jobId}'].");
                    return;
                }

                outboxMessageSet.RemoveRange(expiredMessages);
                await dbContext.SaveChangesAsync();

                stopwatch.Stop();
                _logger.LogTrace($"Processed {expiredMessages.Count} expired outbox messages in {stopwatch.ElapsedMilliseconds} ms [job id: '{jobId}'].");
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"Processing outbox message [job id: '{jobId}'] failed.");
            }

        }
        private async Task<IList<OutboxMessage>> GetExpired(DbSet<OutboxMessage> outboxMessageSet)
        {
            var now = DateTimeOffset.UtcNow.DateTime.Subtract(TimeSpan.FromHours(_options.ExpiryHours));
            var messages = await outboxMessageSet
                .Where(x=>x.ProcessedDate != null && x.ProcessedDate.Value < now)
                .ToListAsync();
            return messages;
        }
    }
}