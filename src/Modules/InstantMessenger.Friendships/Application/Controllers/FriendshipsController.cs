using System;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Application.Features.AcceptInvitation;
using InstantMessenger.Friendships.Application.Features.CancelInvitation;
using InstantMessenger.Friendships.Application.Features.RejectInvitation;
using InstantMessenger.Friendships.Application.Features.RemoveFromFriendships;
using InstantMessenger.Friendships.Application.Features.SendInvitation;
using InstantMessenger.Friendships.Application.Queries;
using InstantMessenger.Shared.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.Friendships.Application.Controllers
{
    [ApiController]
    [Route("api/friendships")]
    [Authorize]
    public class FriendshipsController : ControllerBase
    {
        private readonly FriendshipsModuleFacade _facade;

        public FriendshipsController(FriendshipsModuleFacade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public async Task<IActionResult> Post(SendFriendshipInvitationApiRequest request)
        {
            await _facade.SendAsync(
                new SendFriendshipInvitationCommand(User.GetUserId(), request.ReceiverNickname));
            return Ok();
        }

        [HttpPost("invitations/accept")]
        public async Task<IActionResult> Post(AcceptFriendshipInvitationApiRequest request)
        {
            await _facade.SendAsync(
                new AcceptFriendshipInvitationCommand(request.InvitationId, User.GetUserId()));
            return Ok();
        }

        [HttpPost("invitations/reject")]
        public async Task<IActionResult> Post(RejectFriendshipInvitationApiRequest request)
        {
            await _facade.SendAsync(
                new RejectFriendshipInvitationCommand(request.InvitationId, User.GetUserId()));
            return Ok();
        }

        [HttpPost("invitations/cancel")]
        public async Task<IActionResult> Post(CancelFriendshipInvitationApiRequest request)
        {
            await _facade.SendAsync(
                new CancelFriendshipInvitationCommand(request.InvitationId, User.GetUserId()));
            return Ok();
        }

        [HttpDelete("{friendshipId}")]
        public async Task<IActionResult> Delete(Guid friendshipId)
        {
            await _facade.SendAsync(
                new RemoveFromFriendshipsCommand(User.GetUserId(), friendshipId));
            return Ok();
        }


        [HttpGet("invitations/pending")]
        public async Task<IActionResult> GetPending()
        {
            var result = await _facade.QueryAsync(new GetAllPendingInvitationsQuery(User.GetUserId()));

            return Ok(result);
        }

        [HttpGet("invitations/pending/incoming")]
        public async Task<IActionResult> GetIncomingPending()
        {
            var result = await _facade.QueryAsync(new GetIncomingPendingInvitationsQuery(User.GetUserId()));

            return Ok(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetFriendships()
        {
            var result = await _facade.QueryAsync(new GetFriendshipsQuery(User.GetUserId()));

            return Ok(result);
        }
    }
}