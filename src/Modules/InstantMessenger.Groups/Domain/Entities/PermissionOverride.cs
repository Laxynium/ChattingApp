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
        private static readonly ImmutableHashSet<Permission> _validPermissions = ImmutableHashSet<Permission>.Empty
            .Add(Permission.ManageRoles)
            .Add(Permission.ManageChannels)
            //.Add(Permission.AddReactions)
            //.Add(Permission.EmbedLinks)
            //.Add(Permission.AttachFiles)
            //.Add(Permission.MentionRoles)
            .Add(Permission.SendMessages)
            .Add(Permission.ReadMessages);

        public ChannelId ChannelId { get; }
        public Permission Permission { get; }
        public OverrideType Type { get; }

        protected PermissionOverride(ChannelId channelId, Permission permission, OverrideType type)
        {
            if (!_validPermissions.Contains(permission))
                throw new InvalidPermissionOverride(permission.Name);
            ChannelId = channelId;
            Permission = permission;
            Type = type;
        }

        public Permissions Apply(Permissions permissions)
        {
            return Type == OverrideType.Allow ? permissions.Add(Permission) : permissions.Remove(Permission);
        }

        public static IReadOnlyList<Permission> ValidPermissions => _validPermissions.ToImmutableList();
    }
    public class RolePermissionOverride : PermissionOverride
    {
        public RoleId RoleId { get; }

        public RolePermissionOverride(ChannelId channelId, RoleId roleId, Permission permission, OverrideType type) : base(channelId, permission, type)
        {
            RoleId = roleId;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ChannelId;
            yield return RoleId;
            yield return Permission;
        }
    }

    public class MemberPermissionOverride : PermissionOverride
    {
        public UserId UserIdOfMember { get; }

        public MemberPermissionOverride(ChannelId channelId, UserId userIdOfMember, Permission permission, OverrideType type) : base(channelId,permission, type)
        {
            UserIdOfMember = userIdOfMember;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return ChannelId;
            yield return UserIdOfMember;
            yield return Permission;
        }
    }
}