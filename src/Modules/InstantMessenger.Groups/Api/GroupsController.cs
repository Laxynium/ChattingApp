using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Features.Group.Delete;
using InstantMessenger.Groups.Api.Features.Invitations.GenerateInvitationCode;
using InstantMessenger.Groups.Api.Features.Invitations.JoinGroup;
using InstantMessenger.Groups.Api.Features.Invitations.RevokeInvitation;
using InstantMessenger.Groups.Api.Features.Members.AssignRole;
using InstantMessenger.Groups.Api.Features.Members.Kick;
using InstantMessenger.Groups.Api.Features.Members.LeaveGroup;
using InstantMessenger.Groups.Api.Features.Members.RemoveRole;
using InstantMessenger.Groups.Api.Features.Roles.AddPermissionToRole;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;
using InstantMessenger.Groups.Api.Features.Roles.MoveDownRoleInHierarchy;
using InstantMessenger.Groups.Api.Features.Roles.MoveUpRoleInHierarchy;
using InstantMessenger.Groups.Api.Features.Roles.RemovePermissionFromRole;
using InstantMessenger.Groups.Api.Features.Roles.RemoveRole;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Shared.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.Groups.Api
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly GroupsModuleFacade _facade;

        public GroupsController(GroupsModuleFacade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateGroupApiRequest request)
        {
            await _facade.SendAsync(new CreateGroupCommand(User.GetUserId(),request.GroupId, request.GroupName));
            return Ok();
        }   
        
        [HttpDelete("{groupId}")]
        public async Task<IActionResult> Delete(Guid groupId)
        {
            await _facade.SendAsync(new RemoveGroupCommand(User.GetUserId(),groupId));
            return Ok();
        }

        [HttpPost("{groupId}/leave")]
        public async Task<IActionResult> Post(Guid groupId)
        {
            await _facade.SendAsync(new LeaveGroupCommand(User.GetUserId(),groupId));
            return Ok();
        }

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetGroup([FromRoute] Guid groupId)
        {
            var result = await _facade.QueryAsync(new GetGroupsQuery(User.GetUserId(), groupId));
            return Ok(result.FirstOrDefault());
        }

        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            var result = await _facade.QueryAsync(new GetGroupsQuery(User.GetUserId()));
            return Ok(result);
        }


        [HttpGet("{groupId}/owner")]
        public async Task<IActionResult> GetOwner([FromRoute] Guid groupId)
        {
            var result = await _facade.QueryAsync(new GetMembersQuery(User.GetUserId(), groupId, true));
            return Ok(result.FirstOrDefault());
        }


        [HttpPost("{groupId}/roles")]
        public async Task<IActionResult> Post(AddRoleApiRequest request)
        {
            await _facade.SendAsync(new AddRoleCommand(User.GetUserId(), request.GroupId, request.RoleId, request.Name));
            return Ok();
        }

        [HttpDelete("{groupId}/roles/{roleId}")]
        public async Task<IActionResult> DeleteRole([FromRoute]RemoveRoleApiRequest request)
        {
            await _facade.SendAsync(new RemoveRoleCommand(User.GetUserId(), request.GroupId, request.RoleId));
            return Ok();
        }

        [HttpPut("{groupId}/roles/move-up")]
        public async Task<IActionResult> MoveUpRole(MoveUpRoleApiRequest request)
        {
            await _facade.SendAsync(new MoveUpRoleInHierarchyCommand(User.GetUserId(), request.GroupId, request.RoleId));
            return Ok();
        }

        [HttpPut("{groupId}/roles/move-down")]
        public async Task<IActionResult> MoveDownRole(MoveDownRoleApiRequest request)
        {
            await _facade.SendAsync(new MoveDownRoleInHierarchyCommand(User.GetUserId(), request.GroupId, request.RoleId));
            return Ok();
        }

        [HttpGet("{groupId}/roles")]
        public async Task<IActionResult> GetRoles(Guid groupId)
        {
            var result = await _facade.QueryAsync(new GetRolesQuery(User.GetUserId(), groupId));
            return Ok(result);
        }        
        
        [HttpGet("{groupId}/roles/{roleId}")]
        public async Task<IActionResult> GetRole(Guid groupId, Guid roleId)
        {
            var result = await _facade.QueryAsync(new GetRoleQuery(User.GetUserId(), groupId, roleId));
            return Ok(result);
        }

        [HttpPost("{groupId}/roles/{roleId}/permissions")]
        public async Task<IActionResult> Post(AddPermissionToRoleApiRequest request)
        {
            await _facade.SendAsync(new AddPermissionToRoleCommand(User.GetUserId(), request.GroupId, request.RoleId,request.PermissionName));
            return Ok();
        }

        [HttpDelete("{groupId}/roles/{roleId}/permissions/{permissionName}")]
        public async Task<IActionResult> Delete([FromRoute] RemovePermissionFromRoleApiRequest request)
        {
            await _facade.SendAsync(new RemovePermissionFromRoleCommand(User.GetUserId(), request.GroupId, request.RoleId, request.PermissionName));
            return Ok();
        }

        [HttpGet("{groupId}/permissions")]
        public async Task<IActionResult> GetAvailablePermissions(Guid groupId)
        {
            var result = await _facade.QueryAsync(new GetAvailablePermissionsQuery(groupId));
            return Ok(result);
        }


        [HttpGet("{groupId}/roles/{roleId}/permissions")]
        public async Task<IActionResult> GetPermissions(Guid groupId, Guid roleId)
        {
            var result = await _facade.QueryAsync(new GetRolePermissionsQuery(groupId, roleId));
            return Ok(result);
        }

        [HttpDelete("{groupId}/members/{memberUserId}/kick")]
        public async Task<IActionResult> Delete(Guid groupId, Guid memberUserId)
        {
            await _facade.SendAsync(new KickMemberCommand(User.GetUserId(), groupId, memberUserId));
            return Ok();
        }


        [HttpPost("{groupId}/members/{memberUserId}/roles")]
        public async Task<IActionResult> Post(AssignRoleToMemberApiRequest request)
        {
            await _facade.SendAsync(new AssignRoleToMemberCommand(User.GetUserId(), request.GroupId, request.MemberUserId, request.RoleId));
            return Ok();
        }

        [HttpDelete("{groupId}/members/{memberUserId}/roles/{roleId}")]
        public async Task<IActionResult> RemoveRole([FromRoute]RemoveRoleFromMemberApiRequest request)
        {
            await _facade.SendAsync(new RemoveRoleFromMemberCommand(User.GetUserId(), request.GroupId, request.MemberUserId, request.RoleId));
            return Ok();
        }


        [HttpGet("{groupId}/members/{memberUserId}/roles")]
        public async Task<IActionResult> GetMemberRoles(Guid groupId, Guid memberUserId)
        {
            var result = await _facade.QueryAsync(new GetMemberRolesQuery(User.GetUserId(),groupId, memberUserId));
            return Ok(result);
        }

        [HttpGet("{groupId}/members/{memberUserId}")]
        public async Task<IActionResult> GetMember(Guid groupId, Guid memberUserId)
        {
            var result = await _facade.QueryAsync(new GetMembersQuery(User.GetUserId(), groupId,false,memberUserId));
            return Ok(result.FirstOrDefault());
        }

        [HttpGet("{groupId}/members/me")]
        public async Task<IActionResult> GetMember(Guid groupId)
        {
            var result = await _facade.QueryAsync(new GetMembersQuery(User.GetUserId(), groupId,false,User.GetUserId()));
            return Ok(result.FirstOrDefault());
        }

        [HttpGet("{groupId}/members")]
        public async Task<IActionResult> GetMembers(Guid groupId)
        {
            var result = await _facade.QueryAsync(new GetMembersQuery(User.GetUserId(),groupId,false));
            return Ok(result);
        }

        [HttpPost("{groupId}/invitations")]
        public async Task<IActionResult> GenerateInvitation(GenerateInvitationApiRequest request)
        {
            await _facade.SendAsync(
                new GenerateInvitationCommand(
                    User.GetUserId(),
                    request.GroupId,
                    request.InvitationId,
                    request.ExpirationTime,
                    request.UsageCounter
                )
            );
            return Ok();
        }        

        [HttpDelete("{groupId}/invitations/{invitationId}")]
        public async Task<IActionResult> RevokeInvitation(Guid groupId, Guid invitationId)
        {
            await _facade.SendAsync(
                new RevokeInvitationCommand(
                    User.GetUserId(),
                    groupId,
                    invitationId
                )
            );
            return Ok();
        }

        [HttpGet("{groupId}/invitations/{invitationId}")]
        public async Task<IActionResult> GetInvitation(Guid groupId, Guid invitationId)
        {
            var result = await _facade.QueryAsync(new GetInvitationQuery(User.GetUserId(), groupId, invitationId));
            return Ok(result);
        }

        [HttpGet("{groupId}/invitations")]
        public async Task<IActionResult> GetInvitations(Guid groupId)
        {
            var result = await _facade.QueryAsync(new GetInvitationsQuery(User.GetUserId(), groupId));
            return Ok(result);
        }

        [HttpPost("join/{code}")]
        public async Task<IActionResult> JoinGroup(string code)
        {
            await _facade.SendAsync(new JoinGroupCommand(User.GetUserId(),code));
            return Ok();
        }
    }
}