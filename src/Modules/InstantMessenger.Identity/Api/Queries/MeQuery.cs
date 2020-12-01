using System;
using InstantMessenger.Shared.Queries;

namespace InstantMessenger.Identity.Api.Queries
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Nickname { get; set; }
        public string Avatar { get; set; } //base64 encoded image byte[]
    }
    public class MeQuery : IQuery<UserDto>
    {
        public Guid UserId { get; }

        public MeQuery(string userId)
        {
            UserId = Guid.Parse(userId);
        }
    }
}