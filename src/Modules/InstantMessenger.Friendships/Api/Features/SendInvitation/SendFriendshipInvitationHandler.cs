using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Shared.Commands;
using NodaTime;

namespace InstantMessenger.Friendships.Api.Features.SendInvitation
{
    internal sealed class SendFriendshipInvitationHandler : ICommandHandler<SendFriendshipInvitationCommand>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public SendFriendshipInvitationHandler(
            IPersonRepository personRepository, 
            IInvitationRepository invitationRepository,
            IUnitOfWork unitOfWork,
            IClock clock)
        {
            _personRepository = personRepository;
            _invitationRepository = invitationRepository;
            _unitOfWork = unitOfWork;
            _clock = clock;
        }
        public async Task HandleAsync(SendFriendshipInvitationCommand command)
        {
            var sender = await _personRepository.GetAsync(command.SenderId) ?? throw new PersonNotFoundException();
            var receiver = await _personRepository.GetAsync(command.ReceiverId) ?? throw new PersonNotFoundException();

            var invitation = Invitation.Create(sender.Id, receiver.Id, _clock);

            await _invitationRepository.AddAsync(invitation);

            await _unitOfWork.Commit();
        }
    }
}