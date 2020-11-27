using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Messages;
using InstantMessenger.Groups.Domain.Messages.Entities;

namespace InstantMessenger.Groups.Infrastructure.Database
{
    internal sealed class MessageRepository : IMessageRepository
    {
        private readonly GroupsContext _context;

        public MessageRepository(GroupsContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Message message)
        {
            await _context.Messages.AddAsync(message);
        }
    }
}