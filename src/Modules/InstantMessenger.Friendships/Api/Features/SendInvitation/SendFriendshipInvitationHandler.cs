using System.Threading.Tasks;
using InstantMessenger.Friendships.Api.Features.SendInvitation.ExternalQuery;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Friendships.Domain.Exceptions;
using InstantMessenger.Friendships.Domain.Repositories;
using InstantMessenger.Friendships.Domain.Rules;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Modules;
using NodaTime;

namespace InstantMessenger.Friendships.Api.Features.SendInvitation
{
    internal sealed class SendFriendshipInvitationHandler : ICommandHandler<SendFriendshipInvitationCommand>
    {
        private readonly IPersonRepository _personRepository;
        private readonly IInvitationRepository _invitationRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUniquePendingInvitationRule _rule;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;
        private readonly IModuleClient _moduleClient;

        public SendFriendshipInvitationHandler(
            IPersonRepository personRepository, 
            IInvitationRepository invitationRepository,
            IFriendshipRepository friendshipRepository,
            IUniquePendingInvitationRule rule,
            IUnitOfWork unitOfWork,
            IClock clock,
            IModuleClient moduleClient)
        {
            _personRepository = personRepository;
            _invitationRepository = invitationRepository;
            _friendshipRepository = friendshipRepository;
            _rule = rule;
            _unitOfWork = unitOfWork;
            _clock = clock;
            _moduleClient = moduleClient;
        }

        public async Task HandleAsync(SendFriendshipInvitationCommand command)
        {
            var sender = await _personRepository.GetAsync(command.SenderId) ?? throw new PersonNotFoundException();

            var receiverDto = await _moduleClient.GetAsync<UserDto>("/identity/users", new GetUserQuery(command.ReceiverNickname));
            if(receiverDto is null)
                throw new PersonNotFoundException();

            var receiver = await _personRepository.GetAsync(receiverDto.Id) ?? throw new PersonNotFoundException();

            if (await _friendshipRepository.ExistsBetweenAsync(sender.Id, receiver.Id))
                throw new InvalidInvitationException();

            var invitation = await Invitation.Create(sender, receiver, _rule, _clock);

            await _invitationRepository.AddAsync(invitation);

            await _unitOfWork.Commit();
        }
    }
}