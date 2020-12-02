using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Friendships.Api.Hubs
{
    [Authorize]
    public class FriendshipDto
    {
        public Guid Id { get; }
        public Guid FirstPerson { get; }
        public Guid SecondPerson { get; }
        public DateTimeOffset CreatedAt { get; }

        public FriendshipDto(Guid id, Guid firstPerson, Guid secondPerson, DateTimeOffset createdAt)
        {
            Id = id;
            FirstPerson = firstPerson;
            SecondPerson = secondPerson;
            CreatedAt = createdAt;
        }
    }
    public interface IFriendshipsInterface
    {
        Task OnFriendshipCreated(FriendshipDto friendship);
    }
    public class FriendshipsHub : Hub<IFriendshipsInterface>
    {
    }
}