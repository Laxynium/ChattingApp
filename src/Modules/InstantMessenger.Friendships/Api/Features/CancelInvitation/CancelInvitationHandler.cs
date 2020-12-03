using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Friendships.Domain.Events;
using InstantMessenger.Friendships.Domain.Exceptions;
using InstantMessenger.Friendships.Domain.Repositories;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.MessageBrokers;

namespace InstantMessenger.Friendships.Api.Features.CancelInvitation
{
    internal sealed class CancelInvitationHandler : ICommandHandler<CancelFriendshipInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageBroker _messageBroker;

        public CancelInvitationHandler(IInvitationRepository invitationRepository,
            IPersonRepository personRepository,
            IUnitOfWork unitOfWork,
            IMessageBroker messageBroker)
        {
            _invitationRepository = invitationRepository;
            _personRepository = personRepository;
            _unitOfWork = unitOfWork;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(CancelFriendshipInvitationCommand command)
        {
            var invitation = await _invitationRepository.GetAsync(command.InvitationId) ?? throw new FriendshipInvitationNotFound();
            var person = await _personRepository.GetAsync(command.ReceiverId) ?? throw new PersonNotFoundException();

            invitation.CancelInvitation(person);

            await _unitOfWork.Commit();

            await _messageBroker.PublishAsync(
                new FriendshipInvitationCanceledEvent(invitation.Id, invitation.SenderId, invitation.ReceiverId, invitation.CreatedAt)
            );
        }
    }
}