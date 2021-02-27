using System;
using System.Threading.Tasks;
using InstantMessenger.Shared.Modules;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace InstantMessenger.Shared.Outbox.EFCore
{
    internal class EFCoreMessageOutbox<TModule> : IMessageOutbox<TModule> where TModule : IModule
    {
        private static readonly JsonSerializerSettings SerializerSettings = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };
        
        private readonly DbContext _context;

        public EFCoreMessageOutbox(DbContext context)
        {
            _context = context;
        }
        public async Task AddAsync<T>(T message)
        {
            var serialized = JsonConvert.SerializeObject(message, SerializerSettings);
            var outboxMessageSet = _context.Set<OutboxMessage>();
            var outboxMessage = new OutboxMessage(Guid.NewGuid(), DateTime.UtcNow, serialized);
            await outboxMessageSet.AddAsync(outboxMessage);
        }
    }
}