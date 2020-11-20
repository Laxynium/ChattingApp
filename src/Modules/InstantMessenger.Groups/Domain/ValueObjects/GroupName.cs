using System;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.Exceptions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class GroupName : SimpleValueObject<string>
    {
        private GroupName():base(default){}
        private GroupName(string value) : base(value)
        {
        }

        public static GroupName Create(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new InvalidGroupNameException(value);
            return new GroupName(value);
        }
    }
}