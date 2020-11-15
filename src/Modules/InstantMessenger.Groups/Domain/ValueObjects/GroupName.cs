using System;
using CSharpFunctionalExtensions;

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
                throw new ArgumentException($"Group name cannot be null or whitespace");
            return new GroupName(value);
        }
    }
}