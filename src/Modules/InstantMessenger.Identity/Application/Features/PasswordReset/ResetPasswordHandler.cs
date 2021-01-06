using System.Threading.Tasks;
using InstantMessenger.Identity.Application.Features.SignIn;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Identity.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;
using Microsoft.AspNetCore.Identity;

namespace InstantMessenger.Identity.Application.Features.PasswordReset
{
    internal sealed class ResetPasswordHandler : ICommandHandler<ResetPasswordCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthTokenService _tokenService;

        public ResetPasswordHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IAuthTokenService tokenService)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
        }
        public async Task HandleAsync(ResetPasswordCommand command)
        {
            var user = await _userRepository.GetAsync(new UserId(command.UserId));
            if (user is null)
            {
                return;
            }

            var result = _tokenService.Verify(command.Token, user.PasswordHash, out var userId);
            if (!result)
            {
                return;
            }

            if (userId != user.Id)
            {
                return;
            }

            user.Change(Password.Create(command.Password), _passwordHasher);
        }
    }
}