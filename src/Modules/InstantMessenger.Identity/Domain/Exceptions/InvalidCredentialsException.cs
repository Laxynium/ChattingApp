using System;

namespace InstantMessenger.Identity.Domain.Exceptions
{
    internal class InvalidCredentialsException : DomainException
    {
        public override string Code => "invalid_credentials";

        public InvalidCredentialsException() : base("Credentials are invalid.")
        {
        }
    }
}