using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Messages.Entities;
using InstantMessenger.Groups.Domain.Messages.ValueObjects;

namespace InstantMessenger.Groups.Domain.Messages
{
    public interface IMessageRepository
    {
        Task AddAsync(Message message);
        Task<bool> ExistsAsync(ChannelId channelId, MessageId messageId);
    }
}