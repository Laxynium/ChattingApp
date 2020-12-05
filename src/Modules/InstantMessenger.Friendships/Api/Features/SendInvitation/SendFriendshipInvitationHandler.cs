using System.Threading.Tasks;
using InstantMessenger.Friendships.Api.Features.SendInvitation.ExternalQuery;
using InstantMessenger.Friendships.Domain.Entities;
using InstantMessenger.Friendships.Domain.Exceptions;
using InstantMessenger.Friendships.Domain.Repositories;
using InstantMessenger.Friendships.Domain.Rules;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.Modules;
using NodaTime;

namespace InstantMessenger.Friendships.Api.Features.SendInvitation
{
    internal sealed class SendFriendshipInvitationHandler : ICommandHandler<SendFriendshipInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUniquePendingInvitationRule _rule;
        private readonly IClock _clock;
        private readonly IModuleClient _moduleClient;

        public SendFriendshipInvitationHandler(
            IInvitationRepository invitationRepository,
            IFriendshipRepository friendshipRepository,
            IUniquePendingInvitationRule rule,
            IClock clock,
            IModuleClient moduleClient)
        {
            _invitationRepository = invitationRepository;
            _friendshipRepository = friendshipRepository;
            _rule = rule;
            _clock = clock;
            _moduleClient = moduleClient;
        }

        public async Task HandleAsync(SendFriendshipInvitationCommand command)
        {

            var receiverDto = await _moduleClient.GetAsync<UserDto>("/identity/users", new GetUserQuery(command.ReceiverNickname));
            if(receiverDto is null)
                throw new PersonNotFoundException();

            if (await _friendshipRepository.ExistsBetweenAsync(new PersonId(command.SenderId), new PersonId(receiverDto.Id)))
                throw new InvalidInvitationException();

            var invitation = await Invitation.Create(new PersonId(command.SenderId), new PersonId(receiverDto.Id), _rule, _clock);

            await _invitationRepository.AddAsync(invitation);
        }
    }
}