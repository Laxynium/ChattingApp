using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class RoleAlreadyExistsException : DomainException
    {
        public RoleId RoleId { get; }
        public override string Code => "role_already_exists";

        public RoleAlreadyExistsException(RoleId roleId):base($"Role[id={roleId}] already exists.")
        {
            RoleId = roleId;
        }
    }
}