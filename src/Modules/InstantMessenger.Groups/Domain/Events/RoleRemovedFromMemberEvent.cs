using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Domain.Events
{
    public class RoleRemovedFromMemberEvent : IDomainEvent
    {
        public GroupId GroupId { get; }
        public UserId UserId { get; }
        public MemberId MemberId { get; }
        public RoleId RoleId { get; }
        public RoleName RoleName { get; }
        public RolePriority RolePriority { get; }

        public RoleRemovedFromMemberEvent(GroupId groupId, UserId userId, MemberId memberId, RoleId roleId,
            RoleName roleName, RolePriority rolePriority)
        {
            GroupId = groupId;
            UserId = userId;
            MemberId = memberId;
            RoleId = roleId;
            RoleName = roleName;
            RolePriority = rolePriority;
        }
    }
}