using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.Exceptions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class RoleName : SimpleValueObject<string>
    {
        public static readonly RoleName EveryOneRole = new("@everyone");
        private RoleName():base(default){}
        private RoleName(string value) : base(value)
        {
        }

        public static RoleName Create(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new InvalidRoleNameException($"Role name cannot be null or whitespace");
            return new RoleName(value);
        }
    }
}