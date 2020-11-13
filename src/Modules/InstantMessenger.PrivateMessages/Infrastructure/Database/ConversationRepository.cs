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
                x.FirstParticipant.Id == firstParticipant.Id && x.SecondParticipant.Id == secondParticipant.Id ||
                x.FirstParticipant.Id == secondParticipant.Id && x.SecondParticipant.Id == firstParticipant.Id);

        public async Task<Conversation> GetAsync(ConversationId id) 
            => await _context.Conversations.FindAsync(id);
    }
}