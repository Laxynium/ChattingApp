using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Shared.Commands;
using NodaTime;

namespace InstantMessenger.Friendships.Api.Features.RejectInvitation
{
    internal sealed class RejectInvitationHandler : ICommandHandler<RejectFriendshipInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RejectInvitationHandler(IInvitationRepository invitationRepository,
            IPersonRepository personRepository,
            IFriendshipRepository friendshipRepository,
            IUnitOfWork unitOfWork,
            IClock clock)
        {
            _invitationRepository = invitationRepository;
            _personRepository = personRepository;
            _friendshipRepository = friendshipRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(RejectFriendshipInvitationCommand command)
        {
            var invitation = await _invitationRepository.GetAsync(command.InvitationId) ?? throw new FriendshipInvitationNotFound();
            var person = await _personRepository.GetAsync(command.ReceiverId) ?? throw new PersonNotFoundException();

            invitation.RejectInvitation(person);

            await _unitOfWork.Commit();
        }
    }
}