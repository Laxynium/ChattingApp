namespace InstantMessenger.Friendships.Api.Features.SendInvitation
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