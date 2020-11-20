using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.Exceptions;

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
                throw new InvalidMemberNameException(value);
            return new MemberName(value);
        }
    }
}