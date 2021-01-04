using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Identity.Domain.ValueObjects
{
    public class Nickname : SimpleValueObject<string>
    {
        private Nickname():base(""){}
        private Nickname(string value) : base(value)
        {
            
        }

        public static Nickname Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Nickname cannot be null or whitespace");
            }
            return new Nickname(value);
        }
    }
}