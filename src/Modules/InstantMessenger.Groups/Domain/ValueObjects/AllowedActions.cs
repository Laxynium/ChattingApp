using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class AllowedActions : ValueObject
    {
        public static readonly AllowedActions All = new("all");
        public static readonly AllowedActions ManageGroup = new("manage_group");
        public static readonly AllowedActions ManageInvitations = new("manage_invitations");
        public static readonly AllowedActions ManageRolesGeneral = new("manage_roles_general");
        public static readonly AllowedActions ManageChannelsGeneral = new("manage_channels_general");
        public static readonly AllowedActions KickMembers = new("kick_members");
        public static AllowedActions ManageRoles(IEnumerable<Guid> channels) => new("manage_roles",true, channels.ToList());
        public static AllowedActions ManageChannels(IEnumerable<Guid> channels) => new("manage_channels",true, channels.ToList());
        public static AllowedActions SendMessages(IEnumerable<Guid>channels) => new("send_messages",true,channels.ToList());
        public static AllowedActions ReadMessages(IEnumerable<Guid>channels) => new("read_messages", true,channels.ToList());
        public string Name { get; }
        public bool IsChannelSpecific { get; } = false;
        public IEnumerable<Guid> Channels { get; }

        public AllowedActions(string name, bool isChannelSpecific = false, IReadOnlyList<Guid>? channels = null)
        {
            Name = name;
            IsChannelSpecific = isChannelSpecific;
            Channels = !isChannelSpecific ? new List<Guid>() : channels ?? new List<Guid>();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return IsChannelSpecific;
            yield return Channels.OrderBy(x=>x.ToString());
        }
    }
}