using System;

namespace InstantMessenger.Friendships.Application.Features.SendInvitation.ExternalQuery
{
    public class UserDto
    {
        public Guid Id { get; }
        public string Nickname { get; }

        public UserDto(Guid id, string nickname)
        {
            Id = id;
            Nickname = nickname;
        }
    }
    public class GetUserQuery
    {
        public string Nickname { get; }

        public GetUserQuery(string nickname)
        {
            Nickname = nickname;
        }
    }
}