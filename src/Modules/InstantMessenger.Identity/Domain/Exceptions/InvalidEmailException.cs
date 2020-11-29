using System;

namespace InstantMessenger.Identity.Domain.Exceptions
{
    public class InvalidEmailException : DomainException
    {
        public override string Code => "invalid_email";

        public InvalidEmailException(string email) : base($"{email} is invalid.")
        {
        }
    }
}