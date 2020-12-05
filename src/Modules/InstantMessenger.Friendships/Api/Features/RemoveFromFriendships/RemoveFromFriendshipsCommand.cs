using System;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Friendships.Domain.Events;
using InstantMessenger.Friendships.Domain.Exceptions;
using InstantMessenger.Friendships.Domain.Repositories;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.MessageBrokers;

namespace InstantMessenger.Friendships.Api.Features.RemoveFromFriendships
{
    public class RemoveFromFriendshipsCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid FriendshipsId { get; }

        public RemoveFromFriendshipsCommand(Guid userId, Guid friendshipsId)
        {
            UserId = userId;
            FriendshipsId = friendshipsId;
        }
    }

    internal sealed class RemoveFromFriendshipsHandler : ICommandHandler<RemoveFromFriendshipsCommand>
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageBroker _messageBroker;

        public RemoveFromFriendshipsHandler(IFriendshipRepository friendshipRepository, IUnitOfWork unitOfWork, IMessageBroker messageBroker)
        {
            _friendshipRepository = friendshipRepository;
            _unitOfWork = unitOfWork;
            _messageBroker = messageBroker;
        }
        public async Task HandleAsync(RemoveFromFriendshipsCommand command)
        {
            var friendship = await _friendshipRepository.GetAsync(command.FriendshipsId) ?? throw new FriendshipNotFoundException(command.FriendshipsId);

            friendship.Remove(new Person(command.UserId));

            await _friendshipRepository.RemoveAsync(friendship);
            await _unitOfWork.Commit();
            await _messageBroker.PublishAsync(new FriendshipRemovedEvent(friendship.Id, friendship.FirstPerson, friendship.SecondPerson, friendship.CreatedAt));
        }
    }
}