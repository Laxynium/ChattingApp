using System;

namespace InstantMessenger.Profiles.Domain.Exceptions
{
    internal class ProfileNotFoundException : DomainException
    {
        public override string Code => "profile_not_found";

        public ProfileNotFoundException() : base("Profile was not found")
        {
        }
    }
}