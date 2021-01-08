using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Repositories
{
    public interface IChannelRepository
    {
        public Task AddAsync(Channel channel);
        Task<Channel> GetAsync(GroupId groupId, ChannelId channelId);
        Task RemoveAsync(Channel channel);
        Task<bool> ExistsAsync(GroupId groupId, ChannelId channelId);
    }
}