using System;
using System.Collections.Generic;

namespace InstantMessenger.Groups.Api.Features.Channel.UpdateRolePermissionOverride
{
    public class UpdateRolePermissionOverrideApiRequest
    {
        public Guid GroupId { get; set; }
        public Guid ChannelId { get; set; }
        public Guid RoleId { get; set; }
        public List<PermissionOverrideCommandItem> Overrides { get; set; }

        public UpdateRolePermissionOverrideApiRequest(Guid groupId, Guid channelId, Guid roleId, List<PermissionOverrideCommandItem> overrides)
        {
            GroupId = groupId;
            ChannelId = channelId;
            RoleId = roleId;
            Overrides = overrides;
        }
        public UpdateRolePermissionOverrideApiRequest(){}
    }
}