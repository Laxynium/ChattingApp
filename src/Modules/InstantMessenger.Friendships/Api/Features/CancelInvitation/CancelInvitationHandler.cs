using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain.Entities;
using InstantMessenger.Friendships.Domain.Exceptions;
using InstantMessenger.Friendships.Domain.Repositories;
using InstantMessenger.Friendships.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Friendships.Api.Features.CancelInvitation
{
    internal sealed class CancelInvitationHandler : ICommandHandler<CancelFriendshipInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;

        public CancelInvitationHandler(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public async Task HandleAsync(CancelFriendshipInvitationCommand command)
        {
            var invitation = await _invitationRepository.GetAsync(new InvitationId(command.InvitationId)) ?? throw new FriendshipInvitationNotFound();

            invitation.CancelInvitation(new PersonId(command.ReceiverId));
        }
    }
}