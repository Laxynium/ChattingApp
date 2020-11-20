﻿using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Members.AssignRole
{
    public class RemoveRoleFromMemberApiRequest
    {
        public Guid GroupId { get; set; }
        public Guid MemberUserId { get; set; }
        public Guid RoleId { get; set; }

        public RemoveRoleFromMemberApiRequest()
        {
            
        }
        public RemoveRoleFromMemberApiRequest(Guid groupId, Guid memberUserId, Guid roleId)
        {
            GroupId = groupId;
            MemberUserId = memberUserId;
            RoleId = roleId;
        }
    }
}