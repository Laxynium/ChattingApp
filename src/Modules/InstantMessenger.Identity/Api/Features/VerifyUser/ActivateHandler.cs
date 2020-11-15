using System.Threading.Tasks;
using InstantMessenger.Identity.Api.Events;
using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Identity.Domain.Exceptions;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Identity.Domain.Rules;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.MessageBrokers;

namespace InstantMessenger.Identity.Api.Features.VerifyUser
{
    internal sealed class ActivateHandler : ICommandHandler<ActivateCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IActivationLinkRepository _activationLinkRepository;
        private readonly IUniqueNicknameRule _uniqueNicknameRule;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageBroker _messageBroker;

        public ActivateHandler(IUserRepository userRepository,
            IActivationLinkRepository activationLinkRepository,
            IUniqueNicknameRule uniqueNicknameRule,
            IUnitOfWork unitOfWork,
            IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _activationLinkRepository = activationLinkRepository;
            _uniqueNicknameRule = uniqueNicknameRule;
            _unitOfWork = unitOfWork;
            _messageBroker = messageBroker;
        }
        public async Task HandleAsync(ActivateCommand command)
        {
            var verificationLink = await _activationLinkRepository.GetAsync(command.UserId) ?? throw new InvalidVerificationTokenException();
            var user = await _userRepository.GetAsync(verificationLink.UserId) ?? throw new InvalidVerificationTokenException();

            await user.Activate(verificationLink, command.Token, Nickname.Create(command.Nickname), _uniqueNicknameRule);

            await _unitOfWork.Commit();

            await _messageBroker.PublishAsync(new AccountActivatedEvent(user.Id, user.Email, user.Nickname));
        }
    }
}