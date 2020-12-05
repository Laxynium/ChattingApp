using System;
using System.Threading.Tasks;

namespace InstantMessenger.PrivateMessages.Domain
{
    public interface IConversationRepository
    {
        Task AddAsync(Conversation conversation);
        Task<bool> ExistsAsync(Participant firstParticipant, Participant secondParticipant);
        Task<Conversation> GetAsync(ConversationId id);
        Task RemoveAsync(Conversation conversation);
    }
}