using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InstantMessenger.Shared.Outbox
{
    public class MessageOutbox<TDbContext> : IMessageOutbox where TDbContext : DbContext
    {
        private static readonly JsonSerializerSettings SerializerSettings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.All,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        private readonly TDbContext _dbContext;
        
        public MessageOutbox(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync<T>(T message)
        {
            var serialized = JsonConvert.SerializeObject(message, SerializerSettings);
            var outboxMessageSet = _dbContext.Set<OutboxMessage>();
            var outboxMessage = new OutboxMessage(Guid.NewGuid(), DateTime.UtcNow, serialized);
            await outboxMessageSet.AddAsync(outboxMessage);
        }
    }
}