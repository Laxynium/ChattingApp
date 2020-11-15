using System;
using InstantMessenger.Shared.Queries;

namespace InstantMessenger.Identity.Api.Queries
{
    public class UserDto
    {
        public Guid Id { get; }
        public string Email { get; }
        public string Nickname { get; }

        public UserDto(Guid id, string email, string nickname)
        {
            Id = id;
            Email = email;
            Nickname = nickname;
        }
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