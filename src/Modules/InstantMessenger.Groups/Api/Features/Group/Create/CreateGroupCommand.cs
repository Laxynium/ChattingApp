using System;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Api.Features.Group.Create
{
    public class CreateGroupCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public string GroupName { get; }

        public CreateGroupCommand(Guid userId, Guid groupId, string groupName)
        {
            UserId = userId;
            GroupId = groupId;
            GroupName = groupName;
        }
    }
}