using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Channel.RemoveOverrideForRole
{
    public class RemoveOverrideForRoleCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public string Permission { get; }
        public Guid RoleId { get; }

        public RemoveOverrideForRoleCommand(Guid userId, Guid groupId, Guid channelId, string permission, Guid roleId)
        {
            UserId = userId;
            GroupId = groupId;
            ChannelId = channelId;
            Permission = permission;
            RoleId = roleId;
        }
    }

    internal sealed class RemoveOverrideForRoleHandler : ICommandHandler<RemoveOverrideForRoleCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelRepository _channelRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveOverrideForRoleHandler(IGroupRepository groupRepository, IChannelRepository channelRepository, IUnitOfWork unitOfWork)
        {
            _groupRepository = groupRepository;
            _channelRepository = channelRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(RemoveOverrideForRoleCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ?? throw new GroupNotFoundException(GroupId.From(command.GroupId));
            var channel = await _channelRepository.GetAsync(group.Id, ChannelId.From(command.ChannelId));

            group.RemoveOverride(UserId.From(command.UserId), channel, Permission.FromName(command.Permission), RoleId.From(command.RoleId));

            await _unitOfWork.Commit();
        }
    }
}