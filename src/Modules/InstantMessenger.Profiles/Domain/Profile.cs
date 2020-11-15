using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Profiles.Domain
{
    public class Profile : Entity<Guid>
    {

        public Avatar Avatar { get; private set; } = null;

        private Profile(){}
        public Profile(Guid userId):base(userId)
        {
        }

        public void Change(Avatar avatar)
        {
            Avatar = avatar;
        }
    }
}