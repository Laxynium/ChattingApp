using System.Threading.Tasks;
using InstantMessenger.Profiles.Api.Features.AvatarChange;
using InstantMessenger.Profiles.Api.Queries;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Mvc;
using InstantMessenger.Shared.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.Profiles.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfilesController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public ProfilesController(ICommandDispatcher commandDispatcher,IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _queryDispatcher.QueryAsync(new GetProfile(User.GetUserId()));
            return result is null ? (IActionResult)NotFound() : Ok(result);
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> Post([FromForm]ChangeAvatarApiRequest request)
        {
            await _commandDispatcher.SendAsync(new ChangeAvatarCommand(User.GetUserId(), request.Image));
            return Ok();
        }
    }
}