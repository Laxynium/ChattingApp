﻿using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Hubs;
using InstantMessenger.Groups.Api.IntegrationEvents;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Groups.Api.Features.Channel.RemoveChannel
{
    public class ChannelRemovedEventHandler : IIntegrationEventHandler<ChannelRemovedEvent>
    {
        private readonly IChannelRepository _channelRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IHubContext<GroupsHub, IGroupsHubContract> _hubContext;

        public ChannelRemovedEventHandler(IChannelRepository channelRepository, IGroupRepository groupRepository, IHubContext<GroupsHub,IGroupsHubContract> hubContext)
        {
            _channelRepository = channelRepository;
            _groupRepository = groupRepository;
            _hubContext = hubContext;
        }
        public async Task HandleAsync(ChannelRemovedEvent @event)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(@event.GroupId));
            
            var members = group.Members;
            var allowedMembers = members.Select(m=>m.UserId.Value.ToString("N")).ToList();
            await _hubContext.Clients.Users(allowedMembers)
                .OnChannelRemoved(new ChannelDto(@event.GroupId, @event.ChannelId, @event.ChannelName));
        }
    }
}