using System.Threading.Tasks;
using InstantMessenger.Identity.Api.Events;
using InstantMessenger.Identity.Domain.Exceptions;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.MessageBrokers;

namespace InstantMessenger.Identity.Api.Features.VerifyUser
{
    internal sealed class ActivateHandler : ICommandHandler<ActivateCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IActivationLinkRepository _activationLinkRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageBroker _messageBroker;

        public ActivateHandler(IUserRepository userRepository, IActivationLinkRepository activationLinkRepository, IUnitOfWork unitOfWork, IMessageBroker messageBroker)
        {
            _userRepository = userRepository;
            _activationLinkRepository = activationLinkRepository;
            _unitOfWork = unitOfWork;
            _messageBroker = messageBroker;
        }
        public async Task HandleAsync(ActivateCommand command)
        {
            var verificationLink = await _activationLinkRepository.GetAsync(command.UserId) ?? throw new InvalidVerificationTokenException();
            var user = await _userRepository.GetAsync(verificationLink.UserId) ?? throw new InvalidVerificationTokenException();

            user.Activate(verificationLink, command.Token);

            await _unitOfWork.Commit();

            await _messageBroker.PublishAsync(new AccountActivatedEvent(user.Id, user.Email));
        }
    }
}