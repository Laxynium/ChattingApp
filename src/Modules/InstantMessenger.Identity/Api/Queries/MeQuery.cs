using System;
using InstantMessenger.Shared.Queries;

namespace InstantMessenger.Identity.Api.Queries
{
    public class UserDto
    {
        public Guid Id { get; }
        public string Email { get; }

        public UserDto(Guid id, string email)
        {
            Id = id;
            Email = email;
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