using System.Linq;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class InvitationCode : SimpleValueObject<string>
    {
        private const string ValidCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        private const int Length = 8;
        private InvitationCode():base(default){}
        private InvitationCode(string value) : base(value)
        {
        }

        public static InvitationCode From(string value)
        {
            if(value.Length != Length)
                throw new InvalidInvitationCodeException(value);
            if (value.Any(x => !ValidCharacters.Contains((char) x)))
                throw new InvalidInvitationCodeException(value);
            return new InvitationCode(value);
        }

        public static InvitationCode Create(RandomStringGenerator generator) =>
            new InvitationCode(generator.Generate(Length, ValidCharacters));
    }
}