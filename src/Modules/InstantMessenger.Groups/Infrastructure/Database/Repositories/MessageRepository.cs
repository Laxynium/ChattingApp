using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Messages;
using InstantMessenger.Groups.Domain.Messages.Entities;
using InstantMessenger.Groups.Domain.Messages.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Infrastructure.Database.Repositories
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

        public async Task<bool> ExistsAsync(ChannelId channelId, MessageId messageId)
        {
            return await _context.Messages.AnyAsync(m => m.ChannelId == channelId && m.Id == messageId);
        }
    }
}