using System;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain.Entities;
using InstantMessenger.Friendships.Domain.Exceptions;
using InstantMessenger.Friendships.Domain.Repositories;
using InstantMessenger.Friendships.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

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

        public RemoveFromFriendshipsHandler(IFriendshipRepository friendshipRepository)
        {
            _friendshipRepository = friendshipRepository;
        }
        public async Task HandleAsync(RemoveFromFriendshipsCommand command)
        {
            var friendship = await _friendshipRepository.GetAsync(new FriendshipId(command.FriendshipsId)) ?? throw new FriendshipNotFoundException(command.FriendshipsId);

            friendship.Remove(new PersonId(command.UserId));

            await _friendshipRepository.RemoveAsync(friendship);
        }
    }
}