using System.Collections.Generic;
using System.Threading.Tasks;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Shared.Outbox
{
    internal interface IOutboxMessageAccessor<TModule> where TModule : IModule
    {
        Task<IList<OutboxMessage>> GetUnsent();
        Task MarkAsProcessed(OutboxMessage message);

        Task<IList<OutboxMessage>> GetExpired();
        Task ClearExpired(IList<OutboxMessage> messages);
    }
}