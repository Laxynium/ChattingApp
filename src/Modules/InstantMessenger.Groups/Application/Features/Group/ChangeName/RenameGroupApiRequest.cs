using System;

namespace InstantMessenger.Groups.Application.Features.Group.ChangeName
{
    public class RenameGroupApiRequest
    {
        public Guid GroupId { get; }
        public string Name { get; }

        public RenameGroupApiRequest(Guid groupId, string name)
        {
            GroupId = groupId;
            Name = name;
        }
    }
}