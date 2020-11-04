using System;

namespace InstantMessenger.Profiles
{
    public class ProfileDto
    {
        public Guid Id { get; }
        public string Nickname { get; }
        public string Avatar { get; } //base64 encoded image byte[]

        public ProfileDto(Guid id, string nickname, string avatar)
        {
            Id = id;
            Nickname = nickname;
            Avatar = avatar;
        }
    }
}