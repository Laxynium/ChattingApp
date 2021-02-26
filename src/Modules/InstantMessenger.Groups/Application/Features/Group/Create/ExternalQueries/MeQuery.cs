using System;

namespace InstantMessenger.Groups.Application.Features.Group.Create.ExternalQueries
{
    public class UserDto
    {
        public Guid UserId { get; }
        public string Nickname { get; }
        public string Avatar { get; }

        public UserDto(Guid userId, string nickname, string avatar)
        {
            UserId = userId;
            Nickname = nickname;
            Avatar = avatar;
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