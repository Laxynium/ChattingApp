using System;
using System.Threading.Tasks;
using InstantMessenger.Identity.Application.Features.ChangeAvatar;
using InstantMessenger.Identity.Application.Features.ChangeNickname;
using InstantMessenger.Identity.Application.Features.PasswordReset;
using InstantMessenger.Identity.Application.Features.SendPasswordReset;
using InstantMessenger.Identity.Application.Features.SignIn;
using InstantMessenger.Identity.Application.Features.SignUp;
using InstantMessenger.Identity.Application.Features.VerifyUser;
using InstantMessenger.Identity.Application.Queries;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.Messages.Queries;
using InstantMessenger.Shared.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.Identity.Application
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IAuthTokensCache _cache;

        public IdentityController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher,IAuthTokensCache cache)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _cache = cache;
        }

        [AllowAnonymous]
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(SignUpCommand command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("activate")]
        public async Task<IActionResult> Activate([FromBody] ActivateCommand command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(SignInCommand command)
        {
            await _commandDispatcher.SendAsync(command);
            var dto = _cache.Get(command.Email);
            return Ok(dto);
        }

        [AllowAnonymous]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> SendPasswordReset(SendPasswordResetCommand command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            await _commandDispatcher.SendAsync(command);
            return Ok();
        }

        [HttpPut("nickname")]
        public async Task<IActionResult> ChangeNickname(ChangeNicknameApiRequest request)
        {
            await _commandDispatcher.SendAsync(new ChangeNicknameCommand(User.GetUserId(), request.Nickname));
            return Ok();
        }

        [HttpPost("avatar")]
        public async Task<IActionResult> ChangeAvatar([FromForm]ChangeAvatarApiRequest request)
        {
            await _commandDispatcher.SendAsync(new ChangeAvatarCommand(User.GetUserId(), request.Image));
            return Ok();
        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            var result = await _queryDispatcher.QueryAsync(new MeQuery(User.Identity.Name));
            return Ok(result);
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUsers(Guid userId)
        {
            var result = await _queryDispatcher.QueryAsync(new GetUserByIdQuery(userId));
            return Ok(result);
        }
    }
}