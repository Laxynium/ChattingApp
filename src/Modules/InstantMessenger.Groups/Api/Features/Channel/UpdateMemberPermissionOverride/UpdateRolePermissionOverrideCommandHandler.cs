using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Channel.UpdateMemberPermissionOverride
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

            var overrideActions = ConvertToOverrideActions(command, group, channel);

            foreach (var action in overrideActions)
            {
                action.Invoke();
            }
        }

        private static IEnumerable<Action> ConvertToOverrideActions(UpdateMemberPermissionOverrideCommand command, Domain.Entities.Group group, Domain.Entities.Channel channel)
        {
            return command.Overrides.Select(x => x.Type switch
            {
                PermissionOverrideTypeCommandItem.Allow => (Action) (() => Allow(Permission.FromName(x.Permission))),
                PermissionOverrideTypeCommandItem.Deny=> () => Deny(Permission.FromName(x.Permission)),
                PermissionOverrideTypeCommandItem.Neutral => () => Remove(Permission.FromName(x.Permission)),
                _ => throw new InvalidPermissionOverride(x.Permission)
            });

            void Allow(Permission p)
                => group.AllowPermission(
                    UserId.From(command.UserId),
                    channel,
                    UserId.From(command.MemberUserId),
                    p
                );

            void Deny(Permission p)
                => group.DenyPermission(
                    UserId.From(command.UserId),
                    channel,
                    UserId.From(command.MemberUserId),
                    p
                );

            void Remove(Permission p)
                => group.RemoveOverride(
                    UserId.From(command.UserId),
                    channel,
                    UserId.From(command.MemberUserId),
                    p
                );
        }
    }
}