using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain.Entities;
using InstantMessenger.Friendships.Domain.Exceptions;
using InstantMessenger.Friendships.Domain.Repositories;
using InstantMessenger.Friendships.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Friendships.Application.Features.RejectInvitation
{
    internal sealed class RejectInvitationHandler : ICommandHandler<RejectFriendshipInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;

        public RejectInvitationHandler(IInvitationRepository invitationRepository)
        {
            _invitationRepository = invitationRepository;
        }

        public async Task HandleAsync(RejectFriendshipInvitationCommand command)
        {
            var invitation = await _invitationRepository.GetAsync(new InvitationId(command.InvitationId)) ?? throw new FriendshipInvitationNotFound();

            invitation.RejectInvitation(new PersonId(command.ReceiverId));
        }
    }
}