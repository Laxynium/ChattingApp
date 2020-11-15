namespace InstantMessenger.Identity.Api.Features.ChangeNickname
{
    public class ChangeNicknameApiRequest
    {
        public string Nickname { get; }

        public ChangeNicknameApiRequest(string nickname)
        {
            Nickname = nickname;
        }
    }
}