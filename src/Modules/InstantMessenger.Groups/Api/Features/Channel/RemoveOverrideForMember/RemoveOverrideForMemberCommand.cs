﻿using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Channel.RemoveOverrideForMember
{
    public class RemoveOverrideForMemberCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public string Permission { get; }
        public Guid MemberUserId { get; }

        public RemoveOverrideForMemberCommand(Guid userId, Guid groupId, Guid channelId, string permission, Guid memberUserId)
        {
            UserId = userId;
            GroupId = groupId;
            ChannelId = channelId;
            Permission = permission;
            MemberUserId = memberUserId;
        }
    }

    internal sealed class RemoveOverrideForMemberHandler : ICommandHandler<RemoveOverrideForMemberCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveOverrideForMemberHandler(IGroupRepository groupRepository, IChannelRepository channelRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _channelRepository = channelRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(RemoveOverrideForMemberCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ?? throw new GroupNotFoundException(GroupId.From(command.GroupId));
            var channel = await _channelRepository.GetAsync(group.Id, ChannelId.From(command.ChannelId));

            group.RemoveOverride(UserId.From(command.UserId), channel, Permission.FromName(command.Permission), UserId.From(command.MemberUserId));

            await _unitOfWork.Commit();
        }
    }
}