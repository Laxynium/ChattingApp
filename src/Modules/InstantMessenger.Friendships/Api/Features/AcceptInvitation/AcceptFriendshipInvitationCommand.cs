using System;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Friendships.Api.Features.AcceptInvitation
{
    internal sealed class AcceptFriendshipInvitationCommand : ICommand
    {
        public Guid InvitationId { get; }
        public Guid ReceiverId { get; }

        public AcceptFriendshipInvitationCommand(Guid invitationId, Guid receiverId)
        {
            InvitationId = invitationId;
            ReceiverId = receiverId;
        }
    }
}