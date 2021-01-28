using System;

namespace InstantMessenger.Groups.Application.ResponseDtos
{
    public class GroupDto
    {
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public Guid OwnerId { get; set; }

        public GroupDto(Guid groupId, string name, DateTimeOffset createdAt, Guid ownerId)
        {
            GroupId = groupId;
            Name = name;
            CreatedAt = createdAt;
            OwnerId = ownerId;
        }
        public GroupDto(){}
    }
}