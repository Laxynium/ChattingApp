using System.Threading.Tasks;
using InstantMessenger.Identity.Domain.Exceptions;
using InstantMessenger.Identity.Domain.Repositories;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Identity.Api.Features.VerifyUser
{
    internal sealed class ActivateHandler : ICommandHandler<ActivateCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IVerificationLinkRepository _verificationLinkRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ActivateHandler(IUserRepository userRepository, IVerificationLinkRepository verificationLinkRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _verificationLinkRepository = verificationLinkRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(ActivateCommand command)
        {
            var verificationLink = await _verificationLinkRepository.GetAsync(command.UserId) ?? throw new InvalidVerificationTokenException();
            var user = await _userRepository.GetAsync(verificationLink.UserId) ?? throw new InvalidVerificationTokenException();

            user.Activate(verificationLink, command.Token);

            await _unitOfWork.Commit();
        }
    }
}