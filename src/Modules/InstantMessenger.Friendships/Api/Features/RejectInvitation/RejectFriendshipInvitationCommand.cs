using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Friendships.Api.Features.RejectInvitation
{
    internal sealed class RejectFriendshipInvitationCommand : ICommand
    {
        public Guid InvitationId { get; }
        public Guid ReceiverId { get; }

        public RejectFriendshipInvitationCommand(Guid invitationId, Guid receiverId)
        {
            InvitationId = invitationId;
            ReceiverId = receiverId;
        }
    }
}