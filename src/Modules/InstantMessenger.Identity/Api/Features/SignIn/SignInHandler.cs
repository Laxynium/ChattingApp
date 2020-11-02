using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Shared.Commands;
using Microsoft.AspNetCore.Identity;

namespace InstantMessenger.Identity.Api.Features.SignIn
{
    internal sealed class SignInHandler : ICommandHandler<SignInCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IAuthTokenService _authTokenService;
        private readonly IAuthTokensCache _tokensCache;

        public SignInHandler(IUserRepository userRepository, IPasswordHasher<User> passwordHasher, IAuthTokenService authTokenService, IAuthTokensCache tokensCache)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _authTokenService = authTokenService;
            _tokensCache = tokensCache;
        }
        public async Task HandleAsync(SignInCommand command)
        {
            var user = await _userRepository.GetAsync(command.Email) ?? throw new InvalidCredentialsException();
            if (!user.IsVerified)
                throw new InvalidCredentialsException();

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, command.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new InvalidCredentialsException();
            }

            var authDto = _authTokenService.Create(user.Id);

            _tokensCache.Set(user.Email,authDto);
        }
    }
}