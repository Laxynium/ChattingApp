using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Friendships.Domain.Exceptions;
using InstantMessenger.Friendships.Domain.Repositories;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.MessageBrokers;
using NodaTime;

namespace InstantMessenger.Friendships.Api.Features.CancelInvitation
{
    internal sealed class CancelInvitationHandler : ICommandHandler<CancelFriendshipInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageBroker _messageBroker;
        private readonly IClock _clock;

        public CancelInvitationHandler(IInvitationRepository invitationRepository,
            IPersonRepository personRepository,
            IFriendshipRepository friendshipRepository,
            IUnitOfWork unitOfWork,
            IMessageBroker messageBroker,
            IClock clock)
        {
            _invitationRepository = invitationRepository;
            _personRepository = personRepository;
            _friendshipRepository = friendshipRepository;
            _unitOfWork = unitOfWork;
            _messageBroker = messageBroker;
            _clock = clock;
        }

        public async Task HandleAsync(CancelFriendshipInvitationCommand command)
        {
            var invitation = await _invitationRepository.GetAsync(command.InvitationId) ?? throw new FriendshipInvitationNotFound();
            var person = await _personRepository.GetAsync(command.ReceiverId) ?? throw new PersonNotFoundException();

            invitation.CancelInvitation(person);

            await _unitOfWork.Commit();
        }
    }
}