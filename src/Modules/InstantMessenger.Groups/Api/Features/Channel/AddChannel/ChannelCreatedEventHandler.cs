using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Hubs;
using InstantMessenger.Groups.Api.IntegrationEvents;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Groups.Api.Features.Channel.AddChannel
{
    public class ChannelCreatedEventHandler : IIntegrationEventHandler<ChannelCreatedEvent>
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IHubContext<GroupsHub, IGroupsHubContract> _hubContext;

        public ChannelCreatedEventHandler(IChannelRepository channelRepository, IGroupRepository groupRepository, IHubContext<GroupsHub,IGroupsHubContract> hubContext)
        {
            _channelRepository = channelRepository;
            _groupRepository = groupRepository;
            _hubContext = hubContext;
        }
        public async Task HandleAsync(ChannelCreatedEvent @event)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(@event.GroupId));
            var channel = await _channelRepository.GetAsync(GroupId.From(@event.GroupId), ChannelId.From(@event.ChannelId));
            
            var members = group.Members;
            var allowedMembers = members.Where(m => group.CanAccessChannel(m.UserId,channel)).Select(m=>m.UserId.Value.ToString("N")).ToList();
            await _hubContext.Clients.Users(allowedMembers)
                .OnChannelCreated(new ChannelDto(@event.GroupId, @event.ChannelId, @event.ChannelName));
        }
    }
}