namespace InstantMessenger.Identity.Application.Features.ChangeNickname
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