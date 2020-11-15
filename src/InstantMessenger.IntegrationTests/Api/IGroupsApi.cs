using System;
using System.Net.Http;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Queries;
using RestEase;

namespace InstantMessenger.IntegrationTests.Api
{
    [Header("User-Agent", "RestEase")]
    public interface IGroupsApi
    {
        [Post("/api/groups")]
        Task<HttpResponseMessage> CreateGroup([Header("Authorization")] string token, [Body] CreateGroupApiRequest request);

        [Get("/api/groups/{groupId}")]
        Task<GroupDto> GetGroup([Header("Authorization")] string token, [Path]Guid groupId);

        [Get("/api/groups/{groupId}/owner")]
        Task<MemberDto> GetOwner([Header("Authorization")] string token, [Path]Guid groupId);
    }
}