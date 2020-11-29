using System;

namespace InstantMessenger.Identity.Domain.Exceptions
{
    public class NicknameInUseException : DomainException
    {
        public override string Code => "nickname_already_used";

        public NicknameInUseException(string nickname) : base($"Nickname {nickname} is already used.")
        {
        }
    }
}