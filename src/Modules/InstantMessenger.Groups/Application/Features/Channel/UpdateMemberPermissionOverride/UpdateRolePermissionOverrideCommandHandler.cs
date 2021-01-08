using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Channel.UpdateMemberPermissionOverride
{
    internal sealed class UpdateMemberPermissionOverrideCommandHandler :ICommandHandler<UpdateMemberPermissionOverrideCommand>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IChannelRepository _channelRepository;

        public UpdateMemberPermissionOverrideCommandHandler(IGroupRepository groupRepository, IChannelRepository channelRepository)
        {
            _groupRepository = groupRepository;
            _channelRepository = channelRepository;
        }
        public async Task HandleAsync(UpdateMemberPermissionOverrideCommand command)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(command.GroupId)) ??
                        throw new GroupNotFoundException(GroupId.From(command.GroupId));
            var channel = await _channelRepository.GetAsync(group.Id, ChannelId.From(command.ChannelId)) ??
                          throw new ChannelNotFoundException(ChannelId.From(command.ChannelId));

            group.UpdateOverrides(UserId.From(command.UserId), channel, UserId.From(command.MemberUserId),
                command.Overrides.Select(o => (Permission.FromName(o.Permission), (PermissionOverrideType)o.Type)));
        }
    }
}