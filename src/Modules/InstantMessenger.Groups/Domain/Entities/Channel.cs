using System.Collections.Generic;
using System.Linq;
using InstantMessenger.Groups.Domain.Events;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.BuildingBlocks;

namespace InstantMessenger.Groups.Domain.Entities
{
    public class Channel : Entity<ChannelId>
    {
        private readonly HashSet<RolePermissionOverride> _rolePermissionOverrides = new();
        public IEnumerable<RolePermissionOverride> RolePermissionOverrides => _rolePermissionOverrides.ToList();

        private readonly HashSet<MemberPermissionOverride> _memberPermissionOverrides = new();
        public IEnumerable<MemberPermissionOverride> MemberPermissionOverrides => _memberPermissionOverrides.ToList();

        public GroupId GroupId { get; }
        public ChannelName Name { get; private set; }

        private Channel(){}

        public Channel(ChannelId channelId, GroupId groupId, ChannelName name):base(channelId)
        {
            GroupId = groupId;
            Name = name;
            Apply(new ChannelCreatedEvent(Id, GroupId, Name));
        }

        public Permissions CalculatePermissions(Member asMember, RoleId everyoneRole)
        {
            var permissions = Permissions.Empty();
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
            => ReplaceOverride(new RolePermissionOverride(Id,role.Id, permission, OverrideType.Allow));

        public void DenyPermission(Role role, Permission permission) 
            => ReplaceOverride(new RolePermissionOverride(Id, role.Id, permission, OverrideType.Deny));

        public void AllowPermission(Member member, Permission permission) 
            => ReplaceOverride(new MemberPermissionOverride(Id, member.UserId, permission, OverrideType.Allow));

        public void DenyPermission(Member member, Permission permission) 
            => ReplaceOverride(new MemberPermissionOverride(Id, member.UserId, permission, OverrideType.Deny));

        public void RemoveOverride(Member member, Permission permission) 
            => RemoveOverride(_memberPermissionOverrides,_memberPermissionOverrides
                .Where(x=>x.UserIdOfMember == member.UserId && x.Permission == permission)
                .ToArray());

        public void RemoveOverride(Role role, Permission permission) 
            => RemoveOverride(_rolePermissionOverrides, _rolePermissionOverrides
                .Where(x=>x.RoleId == role.Id && x.Permission == permission)
                .ToArray());

        public void Remove()
        {
            Apply(new ChannelRemovedEvent(Id, GroupId, Name));
        }

        public void Rename(ChannelName name)
        {
            Name = name;
        }

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

        private static void RemoveOverride<T>(ICollection<T> permissionOverrides, params T[] @overrides) where T : PermissionOverride
        {
            foreach (var @override in @overrides)
            {
                if (permissionOverrides.Contains(@override))
                    permissionOverrides.Remove(@override);
            }
        }
    }
}