namespace InstantMessenger.Friendships.Application.Features.SendInvitation
{
    public class SendFriendshipInvitationApiRequest
    {
        public string ReceiverNickname { get; }

        public SendFriendshipInvitationApiRequest(string receiverNickname)
        {
            ReceiverNickname = receiverNickname;
        }
    }
}