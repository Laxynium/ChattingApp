using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Api.Queries;
using InstantMessenger.Shared.Mvc;
using InstantMessenger.Shared.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.PrivateMessages.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrivateMessagesController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public PrivateMessagesController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet()]
        public async Task<IActionResult> GetConversations()
        {
            var result = await _queryDispatcher.QueryAsync(new GetConversationsQuery(User.GetUserId()));
            return Ok(result);
        }
    }
}