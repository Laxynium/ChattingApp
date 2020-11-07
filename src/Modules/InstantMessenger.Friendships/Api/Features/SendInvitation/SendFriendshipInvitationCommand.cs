using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Friendships.Api.Features.SendInvitation
{
    public class SendFriendshipInvitationCommand : ICommand
    {
        public Guid SenderId { get; }
        public Guid ReceiverId { get; }

        public SendFriendshipInvitationCommand(Guid senderId, Guid receiverId)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
        }
    }
}