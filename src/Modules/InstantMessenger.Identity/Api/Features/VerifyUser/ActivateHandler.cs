using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Exceptions;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Identity.Domain.Rules;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Identity.Api.Features.VerifyUser
{
    internal sealed class ActivateHandler : ICommandHandler<ActivateCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IActivationLinkRepository _activationLinkRepository;
        private readonly IUniqueNicknameRule _uniqueNicknameRule;

        public ActivateHandler(IUserRepository userRepository,
            IActivationLinkRepository activationLinkRepository,
            IUniqueNicknameRule uniqueNicknameRule)
        {
            _userRepository = userRepository;
            _activationLinkRepository = activationLinkRepository;
            _uniqueNicknameRule = uniqueNicknameRule;
        }
        public async Task HandleAsync(ActivateCommand command)
        {
            var verificationLink = await _activationLinkRepository.GetAsync(command.UserId) ?? throw new InvalidVerificationTokenException();
            var user = await _userRepository.GetAsync((UserId) verificationLink.UserId) ?? throw new InvalidVerificationTokenException();

            await user.Activate(verificationLink, command.Token, Nickname.Create(command.Nickname), _uniqueNicknameRule);
        }
    }
}