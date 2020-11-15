using System;

namespace InstantMessenger.Groups.Api.Features.Group.Create.ExternalQueries
{
    public class UserDto
    {
        public Guid UserId { get; }
        public string Nickname { get; }

        public UserDto(Guid userId, string nickname)
        {
            UserId = userId;
            Nickname = nickname;
        }
    }
    public class MeQuery
    {
        public Guid UserId { get; }

        public MeQuery(Guid userId)
        {
            UserId = userId;
        }
    }
}