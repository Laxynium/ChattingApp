using System;
using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class InvalidExpirationTimeException : DomainException
    {
        public override string Code => "invalid_expiration_time";

        public InvalidExpirationTimeException() : base("Expiration time is invalid.")
        {
        }
    }
}