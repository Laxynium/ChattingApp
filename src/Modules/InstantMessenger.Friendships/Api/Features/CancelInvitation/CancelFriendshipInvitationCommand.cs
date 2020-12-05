using System;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Friendships.Api.Features.CancelInvitation
{
    internal sealed class CancelFriendshipInvitationCommand : ICommand
    {
        public Guid InvitationId { get; }
        public Guid ReceiverId { get; }

        public CancelFriendshipInvitationCommand(Guid invitationId, Guid receiverId)
        {
            InvitationId = invitationId;
            ReceiverId = receiverId;
        }
    }
}