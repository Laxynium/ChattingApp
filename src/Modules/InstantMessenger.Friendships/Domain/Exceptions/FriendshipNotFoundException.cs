using System;

namespace InstantMessenger.Friendships.Domain.Exceptions
{
    public class FriendshipNotFoundException : DomainException
    {
        public override string Code => "friendship_not_found";

        public FriendshipNotFoundException(Guid friendshipId) : base($"Friendship[id=${friendshipId}] was not found.")
        {
        }
    }
}