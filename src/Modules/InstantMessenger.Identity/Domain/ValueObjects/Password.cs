using System;
using System.Text.RegularExpressions;
using CSharpFunctionalExtensions;
using InstantMessenger.Identity.Domain.Exceptions;

namespace InstantMessenger.Identity.Domain.ValueObjects
{
    public class Password : SimpleValueObject<string>
    {
        private Password(string value) : base(value)
        {
        }

        public static Password Create(string value)
        {
            if (!Regex.IsMatch(value, "^(?=.*[A-Z].*[A-Z])(?=.*[!@#$&*])(?=.*[0-9].*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,}$",RegexOptions.None,TimeSpan.FromMilliseconds(250)))
            {
                throw new PasswordTooWeakException();
            }
            
            return new Password(value);
        }
    }
}