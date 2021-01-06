using System;

namespace InstantMessenger.Groups.Application.ResponseDtos
{
    public class GroupDto
    {
        public Guid GroupId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public GroupDto(Guid groupId, string name, DateTimeOffset createdAt)
        {
            GroupId = groupId;
            Name = name;
            CreatedAt = createdAt;
        }
        public GroupDto(){}
    }
}