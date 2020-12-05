using System;

namespace InstantMessenger.Groups.Api.Features.Group.Create
{
    public class CreateGroupApiRequest
    {
        public Guid GroupId { get; }
        public string GroupName { get; }

        public CreateGroupApiRequest(Guid groupId, string groupName)
        {
            GroupId = groupId;
            GroupName = groupName;
        }
    }
}