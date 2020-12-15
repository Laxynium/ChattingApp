using System;
using System.Collections.Generic;

namespace InstantMessenger.Groups.Api.Features.Channel.UpdateMemberPermissionOverride
{
    public class UpdateMemberPermissionOverrideApiRequest
    {
        public Guid GroupId { get; set; }
        public Guid ChannelId { get; set; }
        public Guid MemberUserId { get; set; }
        public List<PermissionOverrideCommandItem> Overrides { get; set; }

        public UpdateMemberPermissionOverrideApiRequest(Guid groupId, Guid channelId, Guid memberUserId, List<PermissionOverrideCommandItem> overrides)
        {
            GroupId = groupId;
            ChannelId = channelId;
            MemberUserId = memberUserId;
            Overrides = overrides;
        }
        public UpdateMemberPermissionOverrideApiRequest(){}
    }
}