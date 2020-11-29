using System;

namespace InstantMessenger.Identity.Domain.Exceptions
{
    public class PasswordTooWeakException : DomainException
    {
        public override string Code => "too_weak_password";

        public PasswordTooWeakException() : base("Password is too weak.")
        {
        }
    }
}