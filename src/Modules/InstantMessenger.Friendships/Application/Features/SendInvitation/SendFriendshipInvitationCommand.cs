using System;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Friendships.Application.Features.SendInvitation
{
    public class SendFriendshipInvitationCommand : ICommand
    {
        public Guid SenderId { get; }
        public string ReceiverNickname { get; }

        public SendFriendshipInvitationCommand(Guid senderId, string receiverNickname)
        {
            SenderId = senderId;
            ReceiverNickname = receiverNickname;
        }
    }
}