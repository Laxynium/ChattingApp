using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Shared.Modules;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Shared.Outbox.EFCore
{
    internal sealed class EFCoreOutboxMessageAccessor<TModule> : IOutboxMessageAccessor<TModule> where TModule : IModule
    {
        private readonly DbContext _dbContext;
        private readonly OutboxOptions _options;

        public EFCoreOutboxMessageAccessor(DbContext dbContext, OutboxOptions options)
        {
            _dbContext = dbContext;
            _options = options;
        }

        public async Task<IList<OutboxMessage>> GetUnsent()
        {
            var messages = await _dbContext.Set<OutboxMessage>()
                .Where(x => x.ProcessedDate == null)
                .ToListAsync();
            return messages;
        }

        public async Task<IList<OutboxMessage>> GetExpired()
        {
            var now = DateTimeOffset.UtcNow.DateTime.Subtract(TimeSpan.FromHours(_options.ExpiryHours));
            var messages = await _dbContext.Set<OutboxMessage>()
                .Where(x => x.ProcessedDate != null && x.ProcessedDate.Value < now)
                .ToListAsync();
            return messages;
        }

        public async Task ClearExpired(IList<OutboxMessage> messages)
        {
            _dbContext.Set<OutboxMessage>().RemoveRange(messages);
            await _dbContext.SaveChangesAsync();
        }

        public async Task MarkAsProcessed(OutboxMessage message)
        {
            message.ProcessedDate = DateTime.UtcNow;
            await _dbContext.SaveChangesAsync();
        }
    }
}