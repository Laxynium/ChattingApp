using System;

namespace InstantMessenger.Profiles.Api.Queries
{
    public class ProfileDto
    {
        public Guid Id { get; }
        public string Avatar { get; } //base64 encoded image byte[]

        public ProfileDto(Guid id, string avatar)
        {
            Id = id;
            Avatar = avatar;
        }
    }
}