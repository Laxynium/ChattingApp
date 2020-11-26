using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Entities
{
    public class Channel : Entity<ChannelId>
    {
        private readonly HashSet<RolePermissionOverride> _rolePermissionOverrides = new HashSet<RolePermissionOverride>();
        public IEnumerable<RolePermissionOverride> RolePermissionOverrides => _rolePermissionOverrides.ToList();

        private readonly HashSet<MemberPermissionOverride> _memberPermissionOverrides = new HashSet<MemberPermissionOverride>();
        public IEnumerable<MemberPermissionOverride> MemberPermissionOverrides => _memberPermissionOverrides.ToList();

        public GroupId GroupId { get; }
        public ChannelName Name { get; }

        private Channel(){}

        public Channel(ChannelId channelId, GroupId groupId, ChannelName name):base(channelId)
        {
            GroupId = groupId;
            Name = name;
        }

        public Permissions CalculatePermissions(Permissions permissions, Member asMember, RoleId everyoneRole)
        {
            var forEveryoneRole = _rolePermissionOverrides
                .Where(x => x.RoleId == everyoneRole)
                .Cast<PermissionOverride>()
                .ToList();
            permissions = Apply(forEveryoneRole, permissions);

            var memberRolesSpecificOverrides = _rolePermissionOverrides
                .Where(x => asMember.Roles.Contains(x.RoleId))
                .Cast<PermissionOverride>()
                .ToList();
            permissions = Apply(memberRolesSpecificOverrides, permissions);

            var memberSpecificOverrides = _memberPermissionOverrides
                .Where(x => x.UserIdOfMember == asMember.UserId)
                .Cast<PermissionOverride>()
                .ToList();
            permissions = Apply(memberSpecificOverrides, permissions);

            return permissions;
        }

        private static Permissions Apply(IReadOnlyCollection<PermissionOverride> overrides, Permissions permissions)
        {
            permissions = overrides
                .Where(x => x.Type == OverrideType.Deny)
                .Aggregate(permissions, (current, denyPermission) => denyPermission.Apply(current));
            permissions = overrides
                .Where(x => x.Type == OverrideType.Allow)
                .Aggregate(permissions, (current, allowPermission) => allowPermission.Apply(current));
            return permissions;
        }

        public void AllowPermission(Role role, Permission permission) 
            => ReplaceOverride(new RolePermissionOverride(role.Id, permission, OverrideType.Allow));

        public void DenyPermission(RoleId roleId, Permission permission) 
            => ReplaceOverride(new RolePermissionOverride(roleId, permission, OverrideType.Deny));

        public void AllowPermission(UserId userIdOfMember, Permission permission) 
            => ReplaceOverride(new MemberPermissionOverride(userIdOfMember, permission, OverrideType.Allow));

        public void DenyPermission(UserId memberId, Permission permission) 
            => ReplaceOverride(new MemberPermissionOverride(memberId, permission, OverrideType.Deny));

        private void ReplaceOverride(RolePermissionOverride @override) => ReplaceOverride(_rolePermissionOverrides, @override);

        private void ReplaceOverride(MemberPermissionOverride @override) => ReplaceOverride(_memberPermissionOverrides, @override);

        private static void ReplaceOverride<T>(ISet<T> permissionOverrides, T @override) where T : PermissionOverride
        {
            if (permissionOverrides.Contains(@override))
            {
                permissionOverrides.Remove(@override);
            }

            permissionOverrides.Add(@override);
        }
    }
}