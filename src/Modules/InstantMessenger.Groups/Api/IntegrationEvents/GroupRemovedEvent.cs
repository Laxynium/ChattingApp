using System;
using System.Collections.Generic;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.IntegrationEvents;

namespace InstantMessenger.Groups.Api.IntegrationEvents
{
    public class GroupCreatedEvent : IIntegrationEvent
    {

    }
    public class GroupRemovedEvent : IIntegrationEvent
    {
        public Guid GroupId { get; }
        public string GroupName { get; }
        public IEnumerable<Guid> AllowedUsers { get; }
        public GroupRemovedEvent(Guid groupId, string groupName, IEnumerable<Guid> allowedUsers)
        {
            GroupId = groupId;
            GroupName = groupName;
            AllowedUsers = allowedUsers;
        }
    }

    public class InvitationCreatedEvent : IIntegrationEvent
    {
        public Guid GroupId { get; }
        public Guid InvitationId { get; }
        public string Code { get; }
        public InvitationCreatedEvent(Guid groupId, Guid invitationId, string code)
        {
            GroupId = groupId;
            InvitationId = invitationId;
            Code = code;
        }
    }

    public class InvitationRevokedEvent : IIntegrationEvent
    {
        public Guid GroupId { get; }
        public Guid InvitationId { get; }
        public string Code { get; }
        public InvitationRevokedEvent(Guid groupId, Guid invitationId, string code)
        {
            GroupId = groupId;
            InvitationId = invitationId;
            Code = code;
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

    }

    public class RoleRemovedFromMemberEvent : IIntegrationEvent
    {

    }

    public class PermissionAddedToRoleEvent : IIntegrationEvent
    {

    }

    public class PermissionRemovedFromRoleEvent : IIntegrationEvent
    {

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

    public class ChannelRenamedEvent : IIntegrationEvent
    {
    }
}