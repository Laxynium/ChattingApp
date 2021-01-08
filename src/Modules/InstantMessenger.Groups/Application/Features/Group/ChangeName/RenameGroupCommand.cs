using System;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Group.ChangeName
{
    public class RenameGroupCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public string Name { get; }

        public RenameGroupCommand(Guid userId, Guid groupId, string name)
        {
            UserId = userId;
            GroupId = groupId;
            Name = name;
        }
    }
}