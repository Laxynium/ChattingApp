using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain
{
    public interface IChannelRepository
    {
        public Task AddAsync(Channel channel);
        Task<Channel> GetAsync(GroupId groupId, ChannelId channelId);
    }
}