using System;

namespace InstantMessenger.Identity.Domain.Exceptions
{
    public class InvalidVerificationTokenException : DomainException
    {
        public override string Code => "invalid_activation_token";

        public InvalidVerificationTokenException() : base("Activation token is invalid.")
        {
        }
    }
}