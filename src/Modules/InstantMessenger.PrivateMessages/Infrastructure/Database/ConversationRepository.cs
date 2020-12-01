using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.PrivateMessages.Infrastructure.Database
{
    internal sealed class ConversationRepository : IConversationRepository
    {
        private readonly PrivateMessagesContext _context;

        public ConversationRepository(PrivateMessagesContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Conversation conversation) 
            => await _context.Conversations.AddAsync(conversation);

        public async Task<bool> ExistsAsync(Participant firstParticipant, Participant secondParticipant) =>
            await _context.Conversations.AnyAsync(x =>
                x.FirstParticipant == firstParticipant && x.SecondParticipant == secondParticipant ||
                x.FirstParticipant == secondParticipant && x.SecondParticipant == firstParticipant);

        public async Task<Conversation> GetAsync(ConversationId id) 
            => await _context.Conversations.FindAsync(id);
    }
}