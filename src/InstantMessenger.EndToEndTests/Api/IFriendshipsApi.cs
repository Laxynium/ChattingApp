using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Application.Features.AcceptInvitation;
using InstantMessenger.Friendships.Application.Features.RejectInvitation;
using InstantMessenger.Friendships.Application.Features.SendInvitation;
using InstantMessenger.Friendships.Application.Queries;
using RestEase;

namespace InstantMessenger.EndToEndTests.Api
{
    [Header("User-Agent", "RestEase")]
    public interface IFriendshipsApi
    {
        [Post("/api/friendships")]
        Task<HttpResponseMessage> SendFriendshipInvitation(
            [Header("Authorization")] string token, 
            [Body]SendFriendshipInvitationApiRequest request);

        [Post("/api/friendships/invitations/accept")]
        Task<HttpRequestMessage> AcceptInvitation([Header("Authorization")] string token, [Body]AcceptFriendshipInvitationApiRequest request);

        [Post("/api/friendships/invitations/reject")]
        Task<HttpRequestMessage> RejectInvitation([Header("Authorization")] string token, [Body]RejectFriendshipInvitationApiRequest request);

        [Get("/api/friendships/invitations/pending/incoming")]
        Task<List<InvitationDto>> GetPendingInvitations([Header("Authorization")] string token);

        [Get("/api/friendships")]
        Task<List<FriendshipDto>> GetFriendships([Header("Authorization")] string token);
    }
}