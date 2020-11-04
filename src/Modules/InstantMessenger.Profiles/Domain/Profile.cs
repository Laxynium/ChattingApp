using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Profiles.Domain
{
    public class Profile : Entity<Guid>
    {
        public Nickname Nickname { get; private set; } = null;

        public Avatar Avatar { get; private set; } = null;

        private Profile(){}
        public Profile(Guid userId):base(userId)
        {
            Nickname = null;
        }

        public void Change(Nickname nickname)
        {
            Nickname = nickname;
        }

        public void ChangeAvatar(Avatar avatar)
        {
            Avatar = avatar;
        }
    }
}