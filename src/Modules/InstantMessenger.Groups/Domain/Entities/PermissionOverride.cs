using System.Collections.Generic;
using System.Collections.Immutable;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Entities
{
    public enum OverrideType
    {
        Allow,
        Deny
    }
    public abstract class PermissionOverride : ValueObject
    {
        private readonly ImmutableHashSet<Permission> _validPermissions = ImmutableHashSet<Permission>.Empty
            .Add(Permission.AddReactions)
            .Add(Permission.EmbedLinks)
            .Add(Permission.AttachFiles)
            .Add(Permission.MentionRoles)
            .Add(Permission.SendMessages)
            .Add(Permission.ReadMessages);
    public Permission Permission { get; }
        public OverrideType Type { get; }

        protected PermissionOverride(Permission permission, OverrideType type)
        {
            if (!_validPermissions.Contains(permission))
                throw new InvalidPermissionOverride(permission.Name);
            Permission = permission;
            Type = type;
        }

        public Permissions Apply(Permissions permissions)
        {
            return Type == OverrideType.Allow ? permissions.Add(Permission) : permissions.Remove(Permission);
        }
    }
    public class RolePermissionOverride : PermissionOverride
    {
        public RoleId RoleId { get; }

        public RolePermissionOverride(RoleId roleId, Permission permission, OverrideType type) : base(permission, type)
        {
            RoleId = roleId;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return RoleId;
            yield return Permission;
        }
    }

    public class MemberPermissionOverride : PermissionOverride
    {
        public UserId UserIdOfMember { get; }

        public MemberPermissionOverride(UserId userIdOfMember, Permission permission, OverrideType type) : base(permission, type)
        {
            UserIdOfMember = userIdOfMember;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserIdOfMember;
            yield return Permission;
        }
    }
}