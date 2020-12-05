using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Domain.Entities;
using InstantMessenger.PrivateMessages.Domain.ValueObjects;

namespace InstantMessenger.PrivateMessages.Infrastructure.Database
{
    internal sealed class MessageRepository : IMessageRepository
    {
        private readonly PrivateMessagesContext _context;

        public MessageRepository(PrivateMessagesContext context) 
            => _context = context;

        public async Task AddAsync(Message message) 
            => await _context.Messages.AddAsync(message);

        public async Task<Message> GetAsync(MessageId id) 
            => await _context.Messages.FindAsync(id);
    }
}