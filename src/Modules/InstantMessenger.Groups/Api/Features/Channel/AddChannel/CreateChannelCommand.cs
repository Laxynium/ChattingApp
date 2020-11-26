using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Channel.AddChannel
{
    public class CreateChannelCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public string ChannelName { get; }

        public CreateChannelCommand(Guid userId, Guid groupId, Guid channelId, string channelName)
        {
            UserId = userId;
            GroupId = groupId;
            ChannelId = channelId;
            ChannelName = channelName;
        }
    }
}