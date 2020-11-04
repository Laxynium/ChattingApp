namespace InstantMessenger.Profiles.Api.Features.NicknameChange
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