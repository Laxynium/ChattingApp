using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Channel.DenyPermissionForRole
{
    internal sealed class DenyPermissionForRoleHandler : ICommandHandler<DenyPermissionForRoleCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelRepository _channelRepository;

        public DenyPermissionForRoleHandler(IGroupRepository groupRepository, IChannelRepository channelRepository)
        {
            _groupRepository = groupRepository;
            _channelRepository = channelRepository;
        }
        public async Task HandleAsync(DenyPermissionForRoleCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ?? throw new GroupNotFoundException(GroupId.From(command.GroupId));
            var channel = await _channelRepository.GetAsync(group.Id, ChannelId.From(command.ChannelId)) ?? throw new ChannelNotFoundException(ChannelId.From(command.ChannelId));

            group.DenyPermission(UserId.From(command.UserId), channel, RoleId.From(command.RoleId), Permission.FromName(command.Permission));
        }
    }
}