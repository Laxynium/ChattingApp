using System;
using System.Threading.Tasks;
using InstantMessenger.Shared.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.Profiles
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfilesController : ControllerBase
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ProfilesController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _queryDispatcher.QueryAsync(new GetProfile(Guid.Parse(User.Identity.Name ?? string.Empty)));
            return result is null ? (IActionResult)NotFound() : Ok(result);
        }
    }
}