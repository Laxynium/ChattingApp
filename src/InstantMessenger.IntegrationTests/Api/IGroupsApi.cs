using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Features.Invitations.GenerateInvitationCode;
using InstantMessenger.Groups.Api.Features.Members.AssignRole;
using InstantMessenger.Groups.Api.Features.Roles.AddPermissionToRole;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;
using InstantMessenger.Groups.Api.Queries;
using RestEase;

namespace InstantMessenger.IntegrationTests.Api
{
    [Header("User-Agent", "RestEase")]
    [Header("Content-Type","application/json")]
    [BasePath("/api/groups")]
    public interface IGroupsApi
    {
        [Post]
        Task<HttpResponseMessage> CreateGroup([Header("Authorization")] string token, [Body] CreateGroupApiRequest request);

        [Post("{groupId}/roles")]
        Task<HttpResponseMessage> CreateRole([Header("Authorization")] string token, [Path]Guid groupId, [Body] AddRoleApiRequest request);

        [Delete("{groupId}/roles/{roleId}")]
        Task<HttpResponseMessage> RemoveRole([Header("Authorization")] string token, [Path]Guid groupId, [Path]Guid roleId);

        [Post("{groupId}/roles/{roleId}/permissions")]
        Task<HttpResponseMessage> AddPermission([Header("Authorization")] string token, [Path]Guid groupId, [Path]Guid roleId, [Body] AddPermissionToRoleApiRequest addPermission);

        [Delete("{groupId}/roles/{roleId}/permissions/{permissionName}")]
        Task<HttpResponseMessage> RemovePermission([Header("Authorization")] string token, [Path] Guid groupId, [Path] Guid roleId, [Path]string permissionName);

        [Post("{groupId}/members/{memberUserId}/roles")]
        Task<HttpResponseMessage> AssignRole([Header("Authorization")] string token, [Path] Guid groupId, [Path] Guid memberUserId, [Body] AssignRoleToMemberApiRequest assignRole);

        [Delete("{groupId}/members/{memberUserId}/roles/{roleId}")]
        Task<HttpResponseMessage> RemoveRoleFromMember([Header("Authorization")] string token, [Path] Guid groupId, [Path] Guid memberUserId, [Path]Guid roleId);

        [Get("{groupId}")]
        Task<GroupDto> GetGroup([Header("Authorization")] string token, [Path]Guid groupId);

        [Get("{groupId}/owner")]
        Task<MemberDto> GetOwner([Header("Authorization")] string token, [Path]Guid groupId);

        [Get("{groupId}/roles")]
        Task<List<RoleDto>> GetRoles([Header("Authorization")] string token, [Path] Guid groupId);

        [Get("{groupId}/members/{memberUserId}/roles")]
        Task<List<RoleDto>> GetMemberRoles([Header("Authorization")] string token, [Path] Guid groupId, [Path] Guid memberUserId);

        [Get("{groupId}/roles/{roleId}/permissions")]
        Task<List<PermissionDto>> GetRolePermissions([Header("Authorization")] string token, [Path] Guid groupId, [Path] Guid roleId);

        [Post("{groupId}/invitations")]
        Task<HttpResponseMessage> GenerateInvitation([Header("Authorization")] string token, [Path] Guid groupId, [Body]GenerateInvitationApiRequest request);

        [Get("{groupId}/invitations/{invitationId}")]
        Task<InvitationDto> GetInvitation([Header("Authorization")] string token, [Path] Guid groupId, [Path]Guid invitationId);

        [Post("join/{code}")]
        Task<HttpResponseMessage> JoinGroup([Header("Authorization")] string token, [Path] string code);

        [Get("{groupId}/members/{userId}")]
        Task<MemberDto> GetMember([Header("Authorization")] string token, [Path] Guid groupId, [Path]Guid userId);
    }
}