using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain.Entities;
using InstantMessenger.Friendships.Domain.Exceptions;
using InstantMessenger.Friendships.Domain.Repositories;
using InstantMessenger.Friendships.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;
using NodaTime;

namespace InstantMessenger.Friendships.Api.Features.AcceptInvitation
{
    internal sealed class AcceptInvitationHandler : ICommandHandler<AcceptFriendshipInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IClock _clock;

        public AcceptInvitationHandler(IInvitationRepository invitationRepository,
            IFriendshipRepository friendshipRepository,
            IClock clock)
        {
            _invitationRepository = invitationRepository;
            _friendshipRepository = friendshipRepository;
            _clock = clock;
        }

        public async Task HandleAsync(AcceptFriendshipInvitationCommand command)
        {
            var invitation = await _invitationRepository.GetAsync(new InvitationId(command.InvitationId)) ?? throw new FriendshipInvitationNotFound();
            if(await _friendshipRepository.ExistsBetweenAsync(invitation.SenderId, invitation.ReceiverId))
                throw new InvalidInvitationException();

            var friendship = invitation.AcceptInvitation(new PersonId(command.ReceiverId), _clock);

            await _friendshipRepository.AddAsync(friendship);
        }
    }
}