﻿using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.MessageBrokers;
using NodaTime;

namespace InstantMessenger.Friendships.Api.Features.AcceptInvitation
{
    internal sealed class AcceptInvitationHandler : ICommandHandler<AcceptFriendshipInvitationCommand>
    {
        private readonly IInvitationRepository _invitationRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageBroker _messageBroker;
        private readonly IClock _clock;

        public AcceptInvitationHandler(IInvitationRepository invitationRepository,
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

        public async Task HandleAsync(AcceptFriendshipInvitationCommand command)
        {
            var invitation = await _invitationRepository.GetAsync(command.InvitationId) ?? throw new FriendshipInvitationNotFound();
            var person = await _personRepository.GetAsync(command.ReceiverId) ?? throw new PersonNotFoundException();

            var friendship = invitation.AcceptInvitation(person, _clock);

            await _friendshipRepository.AddAsync(friendship);
            await _unitOfWork.Commit();

            await _messageBroker.PublishAsync(new FriendshipCreatedEvent(friendship.Id, friendship.FirstPerson,
                friendship.SecondPerson));
        }
    }
}