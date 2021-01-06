using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Friendships.Application.Hubs
{
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

    public class InvitationDto
    {
        public Guid Id { get; }
        public Guid SenderId { get; }
        public Guid ReceiverId { get; }
        public DateTimeOffset CreatedAt { get; }

        public InvitationDto(Guid id, Guid senderId, Guid receiverId, DateTimeOffset createdAt)
        {
            Id = id;
            SenderId = senderId;
            ReceiverId = receiverId;
            CreatedAt = createdAt;
        }
    }

    public interface IFriendshipsContract
    {
        Task OnFriendshipCreated(FriendshipDto friendship);
        Task OnFriendshipInvitationCreated(InvitationDto friendship);
        Task OnFriendshipInvitationAccepted(InvitationDto friendship);
        Task OnFriendshipInvitationRejected(InvitationDto friendship);
        Task OnFriendshipInvitationCanceled(InvitationDto friendship);
        Task OnFriendshipRemoved(FriendshipDto friendship);
    }

    [Authorize]
    public class FriendshipsHub : Hub<IFriendshipsContract>
    {
    }
}