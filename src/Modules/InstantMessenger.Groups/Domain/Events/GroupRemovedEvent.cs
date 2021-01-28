using System;
using System.Collections.Generic;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Domain.Events
{
    public class GroupRemovedEvent : IDomainEvent
    {
        public GroupId GroupId { get; }
        public GroupName GroupName { get; }
        public DateTimeOffset CreatedAt { get; }
        public IEnumerable<Member> AllowedMembers { get; }
        public UserId OwnerId { get; }
        public GroupRemovedEvent(GroupId groupId, GroupName groupName, DateTimeOffset createdAt, IEnumerable<Member> allowedMembers, UserId ownerId)
        {
            GroupId = groupId;
            GroupName = groupName;
            CreatedAt = createdAt;
            AllowedMembers = allowedMembers;
            OwnerId = ownerId;
        }
    }
}