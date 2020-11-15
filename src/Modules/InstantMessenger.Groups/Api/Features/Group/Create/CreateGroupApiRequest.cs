using System;

namespace InstantMessenger.Groups.Api.Features.Group.Create
{
    public class CreateGroupApiRequest
    {
        public Guid GroupId { get; }
        public Guid OwnerId { get; }
        public string GroupName { get; }

        public CreateGroupApiRequest(Guid groupId, Guid ownerId, string groupName)
        {
            GroupId = groupId;
            OwnerId = ownerId;
            GroupName = groupName;
        }
    }
}