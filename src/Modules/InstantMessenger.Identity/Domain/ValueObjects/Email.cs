using System;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using InstantMessenger.Identity.Domain.Exceptions;

namespace InstantMessenger.Identity.Domain.ValueObjects
{
    public class Email : SimpleValueObject<string>
    {
        private Email(string value) : base(value){}

        public static Email Create(string value)
        {
            if (!IsValid(value))
            {
                throw new InvalidEmailException();
            }

            var email = new Email(value);
            
            return email;
        }

        private static bool IsValid(string value)
        {
            try
            {
                return Regex.IsMatch(value,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}