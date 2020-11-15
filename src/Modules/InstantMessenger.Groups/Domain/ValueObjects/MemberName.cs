using System;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class MemberName : SimpleValueObject<string>
    {
        private MemberName():base(default){}
        private MemberName(string value) : base(value)
        {
        }
        public static MemberName Create(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Member name cannot be null or whitespace");
            return new MemberName(value);
        }
    }
}