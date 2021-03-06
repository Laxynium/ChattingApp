﻿using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Channel.DenyPermissionForMember
{
    internal sealed class DenyPermissionForMemberHandler : ICommandHandler<DenyPermissionForMemberCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelRepository _channelRepository;
        public DenyPermissionForMemberHandler(IGroupRepository groupRepository, IChannelRepository channelRepository)
        {
            _groupRepository = groupRepository;
            _channelRepository = channelRepository;
        }

        public async Task HandleAsync(DenyPermissionForMemberCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ??
                        throw new GroupNotFoundException(GroupId.From(command.GroupId));
            var channel = await _channelRepository.GetAsync(group.Id, ChannelId.From(command.ChannelId)) ??
                          throw new ChannelNotFoundException(ChannelId.From(command.ChannelId));

            group.DenyPermission(UserId.From(command.UserId),channel,UserId.From(command.MemberUserId),Permission.FromName(command.Permission));
        }
    }
}