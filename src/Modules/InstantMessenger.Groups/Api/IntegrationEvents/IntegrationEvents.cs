using System;
using System.Collections.Generic;
using InstantMessenger.Groups.Api.ResponseDtos;
using InstantMessenger.Shared.IntegrationEvents;

namespace InstantMessenger.Groups.Api.IntegrationEvents
{
    public class GroupRemovedEvent : IIntegrationEvent
    {
        public Guid GroupId { get; }
        public string GroupName { get; }
        public DateTimeOffset CreatedAt { get; }
        public IEnumerable<Guid> AllowedUsers { get; }
        public GroupRemovedEvent(Guid groupId, string groupName, DateTimeOffset createdAt, IEnumerable<Guid> allowedUsers)
        {
            GroupId = groupId;
            GroupName = groupName;
            CreatedAt = createdAt;
            AllowedUsers = allowedUsers;
        }
    }

    public class InvitationCreatedEvent : IIntegrationEvent
    {
        public Guid GroupId { get; }
        public Guid InvitationId { get; }
        public string Code { get; }
        public ExpirationTimeDto ExpirationTime { get; }
        public UsageCounterDto UsageCounter { get; }
        public InvitationCreatedEvent(Guid groupId, Guid invitationId, string code, ExpirationTimeDto expirationTime, UsageCounterDto usageCounter)
        {
            GroupId = groupId;
            InvitationId = invitationId;
            Code = code;
            ExpirationTime = expirationTime;
            UsageCounter = usageCounter;
        }
    }

    public class InvitationRevokedEvent : IIntegrationEvent
    {
        public Guid GroupId { get; }
        public Guid InvitationId { get; }
        public string Code { get; }
        public ExpirationTimeDto ExpirationTime { get; }
        public UsageCounterDto UsageCounter { get; }
        public InvitationRevokedEvent(Guid groupId, Guid invitationId, string code, ExpirationTimeDto expirationTime, UsageCounterDto usageCounter)
        {
            GroupId = groupId;
            InvitationId = invitationId;
            Code = code;
            ExpirationTime = expirationTime;
            UsageCounter = usageCounter;
        }
    }

    public class MemberAddedToGroupEvent : IIntegrationEvent
    {
    }

    public class MemberKickedFromGroupEvent : IIntegrationEvent
    {

    }

    public class MemberLeftGroupEvent : IIntegrationEvent
    {

    }

    public class RoleCreatedEvent : IIntegrationEvent
    {

    }

    public class RoleRemovedEvent : IIntegrationEvent
    {

    }

    public class RoleRenamedEvent : IIntegrationEvent
    {

    }

    public class RoleAddedToMemberEvent : IIntegrationEvent
    {
        public Guid GroupId{ get; }
        public Guid UserId { get; }
        public Guid RoleId { get; }
        public string RoleName { get; }
        public int RolePriority { get; }

        public RoleAddedToMemberEvent(Guid groupId, Guid userId, Guid roleId, string roleName, int rolePriority)
        {
            GroupId = groupId;
            UserId = userId;
            RoleId = roleId;
            RoleName = roleName;
            RolePriority = rolePriority;
        }
    }

    public class RoleRemovedFromMemberEvent : IIntegrationEvent
    {
        public Guid GroupId { get; }
        public Guid UserId { get; }
        public Guid RoleId { get; }
        public string RoleName { get; }
        public int RolePriority { get; }

        public RoleRemovedFromMemberEvent(Guid groupId, Guid userId, Guid roleId, string roleName, int rolePriority)
        {
            GroupId = groupId;
            UserId = userId;
            RoleId = roleId;
            RoleName = roleName;
            RolePriority = rolePriority;
        }
    }

    public class PermissionAddedToRoleEvent : IIntegrationEvent
    {
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public string PermissionName { get; }
        public int PermissionValue { get; }

        public PermissionAddedToRoleEvent(Guid groupId, Guid roleId, string permissionName, int permissionValue)
        {
            RoleId = roleId;
            PermissionName = permissionName;
            PermissionValue = permissionValue;
            GroupId = groupId;
        }
    }

    public class PermissionRemovedFromRoleEvent : IIntegrationEvent
    {
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public string PermissionName { get; }
        public int PermissionValue { get; }

        public PermissionRemovedFromRoleEvent(Guid groupId, Guid roleId, string permissionName, int permissionValue)
        {
            RoleId = roleId;
            PermissionName = permissionName;
            PermissionValue = permissionValue;
            GroupId = groupId;
        }

    }

    public class ChannelCreatedEvent : IIntegrationEvent
    {
        public Guid ChannelId { get; }
        public Guid GroupId { get; }
        public string ChannelName { get; }

        public ChannelCreatedEvent(Guid channelId, Guid groupId, string channelName)
        {
            ChannelId = channelId;
            GroupId = groupId;
            ChannelName = channelName;
        }
    }

    public class ChannelRemovedEvent : IIntegrationEvent
    {
        public Guid ChannelId { get; }
        public Guid GroupId { get; }
        public string ChannelName { get; }

        public ChannelRemovedEvent(Guid channelId, Guid groupId, string channelName)
        {
            ChannelId = channelId;
            GroupId = groupId;
            ChannelName = channelName;
        }
    }

    public class ChannelRolePermissionOverridesChangedEvent : IIntegrationEvent
    {
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public Guid RoleId { get; }
        public IEnumerable<PermissionOverrideDto> Overrides { get; }

        public ChannelRolePermissionOverridesChangedEvent(Guid groupId, Guid channelId, Guid roleId, IEnumerable<PermissionOverrideDto> overrides)
        {
            GroupId = groupId;
            ChannelId = channelId;
            RoleId = roleId;
            Overrides = overrides;
        }
    }
    public class ChannelMemberPermissionOverridesChangedEvent : IIntegrationEvent
    {
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public Guid UserId { get; }
        public IEnumerable<PermissionOverrideDto> Overrides { get; }

        public ChannelMemberPermissionOverridesChangedEvent(Guid groupId, Guid channelId, Guid userId, IEnumerable<PermissionOverrideDto> overrides)
        {
            GroupId = groupId;
            ChannelId = channelId;
            UserId = userId;
            Overrides = overrides;
        }
    }


    public class ChannelRenamedEvent : IIntegrationEvent
    {
    }

    public class MessageCreatedEvent : IIntegrationEvent
    {
        public Guid MessageId { get; }
        public Guid GroupId { get; }
        public Guid ChannelId { get; }
        public Guid SenderId { get; }
        public string Content { get; }
        public DateTimeOffset CreatedAt { get; }

        public MessageCreatedEvent(Guid messageId, Guid groupId, Guid channelId,
            Guid senderId, string content, DateTimeOffset createdAt)
        {
            ChannelId = channelId;
            SenderId = senderId;
            Content = content;
            CreatedAt = createdAt;
            GroupId = groupId;
            MessageId = messageId;
        }
    }
}