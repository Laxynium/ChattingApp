﻿using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Channel.AllowPermissionForRole
{
    internal sealed class AllowPermissionForRoleHandler : ICommandHandler<AllowPermissionForRoleCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AllowPermissionForRoleHandler(IGroupRepository groupRepository, IChannelRepository channelRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _channelRepository = channelRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(AllowPermissionForRoleCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ??
                        throw new GroupNotFoundException(GroupId.From(command.GroupId));
            var channel = await _channelRepository.GetAsync(group.Id, ChannelId.From(command.ChannelId)) ??
                          throw new ChannelNotFoundException(ChannelId.From(command.ChannelId));

            group.AllowPermission(
                UserId.From(command.UserId),
                channel,
                RoleId.From(command.RoleId),
                Permission.FromName(command.Permission)
            );


            await _unitOfWork.Commit();
        }
    }
}