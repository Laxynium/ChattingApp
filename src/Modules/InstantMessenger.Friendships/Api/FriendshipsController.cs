using System.Threading.Tasks;
using InstantMessenger.Friendships.Api.Features.AcceptInvitation;
using InstantMessenger.Friendships.Api.Features.CancelInvitation;
using InstantMessenger.Friendships.Api.Features.RejectInvitation;
using InstantMessenger.Friendships.Api.Features.SendInvitation;
using InstantMessenger.Friendships.Api.Queries;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Mvc;
using InstantMessenger.Shared.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.Friendships.Api
{
    [ApiController]
    [Route("api/friendships")]
    [Authorize]
    public class FriendshipsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public FriendshipsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpPost]
        public async Task<IActionResult> Post(SendFriendshipInvitationApiRequest request)
        {
            await _commandDispatcher.SendAsync(
                new SendFriendshipInvitationCommand(User.GetUserId(), request.ReceiverNickname));
            return Ok();
        }

        [HttpPost("invitations/accept")]
        public async Task<IActionResult> Post(AcceptFriendshipInvitationApiRequest request)
        {
            await _commandDispatcher.SendAsync(
                new AcceptFriendshipInvitationCommand(request.InvitationId, User.GetUserId()));
            return Ok();
        }

        [HttpPost("invitations/reject")]
        public async Task<IActionResult> Post(RejectFriendshipInvitationApiRequest request)
        {
            await _commandDispatcher.SendAsync(
                new RejectFriendshipInvitationCommand(request.InvitationId, User.GetUserId()));
            return Ok();
        }

        [HttpPost("invitations/cancel")]
        public async Task<IActionResult> Post(CancelFriendshipInvitationApiRequest request)
        {
            await _commandDispatcher.SendAsync(
                new CancelFriendshipInvitationCommand(request.InvitationId, User.GetUserId()));
            return Ok();
        }


        [HttpGet("invitations/pending")]
        public async Task<IActionResult> GetPending()
        {
            var result = await _queryDispatcher.QueryAsync(new GetAllPendingInvitationsQuery(User.GetUserId()));

            return Ok(result);
        }

        [HttpGet("invitations/pending/incoming")]
        public async Task<IActionResult> GetIncomingPending()
        {
            var result = await _queryDispatcher.QueryAsync(new GetIncomingPendingInvitationsQuery(User.GetUserId()));

            return Ok(result);
        }

        [HttpGet()]
        public async Task<IActionResult> GetFriendships()
        {
            var result = await _queryDispatcher.QueryAsync(new GetFriendshipsQuery(User.GetUserId()));

            return Ok(result);
        }
    }
}