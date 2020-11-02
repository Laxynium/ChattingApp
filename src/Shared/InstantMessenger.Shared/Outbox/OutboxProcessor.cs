using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InstantMessenger.Shared.Modules;
using Microsoft.EntityFrameworkCore;
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

        private readonly IModuleClient _moduleClient;
        private readonly TDbContext _dbContext;
        private readonly ILogger<OutboxProcessor<TDbContext>> _logger;
        private Timer _timer;
        private readonly TimeSpan _interval = TimeSpan.FromMilliseconds(100);
        public OutboxProcessor(IModuleClient moduleClient, TDbContext dbContext, ILogger<OutboxProcessor<TDbContext>> logger)
        {
            _moduleClient = moduleClient;
            _dbContext = dbContext;
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
           _timer = new Timer(SendOutboxMessage, null, TimeSpan.Zero, _interval);
           return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;;
        }

        public void SendOutboxMessage(object state)
        {
            _ = SendOutboxMessageAsync();
        }

        public async Task SendOutboxMessageAsync()
        {
            var jobId = Guid.NewGuid().ToString("N");
            _logger.LogTrace($"Started processing outbox message... [job id: '{jobId}']");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var outboxMessageSet = _dbContext.Set<OutboxMessage>();
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
                await _moduleClient.PublishAsync(@event);

                message.ProcessedDate = DateTime.UtcNow;
                outboxMessageSet.Update(message);
                await _dbContext.SaveChangesAsync();
            }
                        
            stopwatch.Stop();
            _logger.LogTrace($"Processed {messages.Count} outbox messages in {stopwatch.ElapsedMilliseconds} ms [job id: '{jobId}'].");
        }

        private async Task<IList<OutboxMessage>> GetUnsent(DbSet<OutboxMessage> outboxMessageSet)
        {
            var messages = await outboxMessageSet
                .Where(x=>x.ProcessedDate == null)
                .ToListAsync();
            return messages;
        }
    }
}