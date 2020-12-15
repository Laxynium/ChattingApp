using System;

namespace InstantMessenger.Groups.Api.Features.Group.ChangeName
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