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
    public class OutboxProcessor<TDbContext> : IHostedService where TDbContext : DbContext
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly IMessageBroker _messageBroker;
        private readonly ILogger<OutboxProcessor<TDbContext>> _logger;
        private readonly IServiceProvider _scopeFactory;
        private readonly OutboxOptions _options;
        private Timer _sendMessagesTimer;
        private Timer _cleanExpiredMessagesTimer;
        private readonly TimeSpan _interval;
        public OutboxProcessor(OutboxOptions options, IMessageBroker messageBroker, IServiceProvider scopeFactory, ILogger<OutboxProcessor<TDbContext>> logger)
        {
            _messageBroker = messageBroker;
            _logger = logger;
            _scopeFactory = scopeFactory;
            _options = options;
            _interval = TimeSpan.FromMilliseconds(options.IntervalMilliseconds);
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (!_options.Enabled)
                return Task.CompletedTask;

            _sendMessagesTimer = new Timer(SendOutboxMessage, null, TimeSpan.Zero, _interval);

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

            _sendMessagesTimer?.Change(Timeout.Infinite, 0);

            _cleanExpiredMessagesTimer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;;
        }

        public void SendOutboxMessage(object state)
        {
            _ = SendOutboxMessageAsync();
        }
        public void CleanOutdatedMessages(object state)
        {
            _ = CleanExpiredMessagesAsync();
        }

        public async Task SendOutboxMessageAsync()
        {
            var jobId = Guid.NewGuid().ToString("N");
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<TDbContext>();
                _logger.LogTrace($"Started processing outbox message... [job id: '{jobId}']");
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                var outboxMessageSet = dbContext.Set<OutboxMessage>();
                var messages = await GetUnsent(outboxMessageSet);
                _logger.LogTrace($"Found {messages.Count} unsent messages in outbox [job id: '{jobId}'].");
                if (!messages.Any())
                {
                    _logger.LogTrace($"No messages to be processed in outbox [job id: '{jobId}'].");
                    return;
                }

                foreach (var message in messages.OrderBy(m => m.OccurredOn))
                {
                    var @event = JsonConvert.DeserializeObject(message.Data, SerializerSettings);
                    await _messageBroker.PublishAsync((IIntegrationEvent) @event);

                    message.ProcessedDate = DateTime.UtcNow;
                    outboxMessageSet.Update(message);
                    await dbContext.SaveChangesAsync();
                }

                stopwatch.Stop();
                _logger.LogTrace($"Processed {messages.Count} outbox messages in {stopwatch.ElapsedMilliseconds} ms [job id: '{jobId}'].");

                var expiredMessages = await GetExpired(outboxMessageSet);
                _logger.LogTrace($"Found {expiredMessages.Count} expired messages in outbox [job id: '{jobId}'].");
                if (expiredMessages.Any())
                {
                    _logger.LogTrace($"Found {expiredMessages.Count} expired messages in outbox [job id: '{jobId}'].");
                    outboxMessageSet.RemoveRange(expiredMessages);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,$"Processing outbox message [job id: '{jobId}'] failed.");
            }

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

        private async Task<IList<OutboxMessage>> GetUnsent(DbSet<OutboxMessage> outboxMessageSet)
        {
            var messages = await outboxMessageSet
                .Where(x=>x.ProcessedDate == null)
                .ToListAsync();
            return messages;
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