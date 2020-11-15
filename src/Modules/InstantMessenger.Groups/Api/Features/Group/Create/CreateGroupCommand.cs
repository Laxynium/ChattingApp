using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Group.Create
{
    public class CreateGroupCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid OwnerId { get; }
        public string GroupName { get; }

        public CreateGroupCommand(Guid userId, Guid groupId, Guid ownerId, string groupName)
        {
            UserId = userId;
            GroupId = groupId;
            OwnerId = ownerId;
            GroupName = groupName;
        }
    }
}