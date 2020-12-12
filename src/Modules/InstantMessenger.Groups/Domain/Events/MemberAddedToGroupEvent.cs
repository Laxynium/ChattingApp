using System;
using System.Collections.Generic;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Messages.ValueObjects;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Domain.Events
{
    public class GroupCreatedEvent : IDomainEvent
    {

    }
    public class GroupRemovedEvent : IDomainEvent
    {
        public GroupId GroupId { get; }
        public GroupName GroupName { get; }
        public IEnumerable<Member> AllowedMembers { get; }
        public GroupRemovedEvent(GroupId groupId, GroupName groupName, IEnumerable<Member> allowedMembers)
        {
            GroupId = groupId;
            GroupName = groupName;
            AllowedMembers = allowedMembers;
        }
    }

    public class InvitationCreatedEvent : IDomainEvent
    {
        public InvitationId InvitationId { get; }
        public GroupId GroupId { get; }
        public InvitationCode InvitationCode { get; }
        public ExpirationTime ExpirationTime { get; }
        public UsageCounter UsageCounter { get; }
        public InvitationCreatedEvent(InvitationId invitationId, GroupId groupId, InvitationCode invitationCode, ExpirationTime expirationTime, UsageCounter usageCounter)
        {
            InvitationId = invitationId;
            GroupId = groupId;
            InvitationCode = invitationCode;
            ExpirationTime = expirationTime;
            UsageCounter = usageCounter;
        }
    }

    public class InvitationRevokedEvent : IDomainEvent
    {
        public InvitationId InvitationId { get; }
        public GroupId GroupId { get; }
        public InvitationCode InvitationCode { get; }
        public ExpirationTime ExpirationTime { get; }
        public UsageCounter UsageCounter { get; }
        public InvitationRevokedEvent(InvitationId invitationId, GroupId groupId, InvitationCode invitationCode, ExpirationTime expirationTime, UsageCounter usageCounter)
        {
            InvitationId = invitationId;
            GroupId = groupId;
            InvitationCode = invitationCode;
            ExpirationTime = expirationTime;
            UsageCounter = usageCounter;
        }
    }

    public class MemberAddedToGroupEvent : IDomainEvent
    {
        
    }

    public class MemberKickedFromGroupEvent : IDomainEvent
    {

    }

    public class MemberLeftGroupEvent : IDomainEvent
    {

    }

    public class RoleCreatedEvent : IDomainEvent
    {

    }

    public class RoleRemovedEvent : IDomainEvent
    {

    }

    public class RoleRenamedEvent : IDomainEvent
    {

    }

    public class RoleAddedToMemberEvent : IDomainEvent
    {

    }

    public class RoleRemovedFromMemberEvent : IDomainEvent
    {

    }

    public class PermissionAddedToRoleEvent : IDomainEvent
    {

    }    

    public class PermissionRemovedFromRoleEvent : IDomainEvent
    {

    }

    public class ChannelCreatedEvent : IDomainEvent
    {
        public ChannelId ChannelId { get; }
        public GroupId GroupId { get; }
        public ChannelName ChannelName { get; }

        public ChannelCreatedEvent(ChannelId channelId, GroupId groupId, ChannelName channelName)
        {
            ChannelId = channelId;
            GroupId = groupId;
            ChannelName = channelName;
        }
    }

    public class ChannelRemovedEvent : IDomainEvent
    {
        public ChannelId ChannelId { get; }
        public GroupId GroupId { get; }
        public ChannelName ChannelName { get; }

        public ChannelRemovedEvent(ChannelId channelId, GroupId groupId, ChannelName channelName)
        {
            ChannelId = channelId;
            GroupId = groupId;
            ChannelName = channelName;
        }
    }

    public class ChannelRenamedEvent : IDomainEvent
    {
    }

    public class MessageCreatedEvent : IDomainEvent
    {
        public MessageId MessageId { get; }
        public GroupId GroupId { get; }
        public ChannelId ChannelId { get; }
        public UserId SenderId { get; }
        public MessageContent Content { get; }
        public DateTimeOffset CreatedAt { get; }

        public MessageCreatedEvent(MessageId messageId, GroupId groupId, ChannelId channelId,
            UserId senderId, MessageContent content, DateTimeOffset createdAt)
        {
            MessageId = messageId;
            GroupId = groupId;
            ChannelId = channelId;
            SenderId = senderId;
            Content = content;
            CreatedAt = createdAt;
        }
    }
}