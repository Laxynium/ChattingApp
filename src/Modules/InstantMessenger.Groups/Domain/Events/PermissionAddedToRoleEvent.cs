using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Domain.Events
{
    public class PermissionAddedToRoleEvent : IDomainEvent
    {
        public GroupId GroupId { get; }
        public RoleId RoleId { get; }
        public string PermissionName { get; }
        public int PermissionValue { get; }

        public PermissionAddedToRoleEvent(GroupId groupId, RoleId roleId, string permissionName, int permissionValue)
        {
            RoleId = roleId;
            PermissionName = permissionName;
            PermissionValue = permissionValue;
            GroupId = groupId;
        }
    }
}