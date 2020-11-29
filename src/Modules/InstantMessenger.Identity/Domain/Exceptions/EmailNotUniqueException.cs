using System;

namespace InstantMessenger.Identity.Domain.Exceptions
{
    public class EmailNotUniqueException : DomainException
    {
        public override string Code => "email_already_used";

        public EmailNotUniqueException(string email) : base($"Email {email} is already used.")
        {
        }
    }
}