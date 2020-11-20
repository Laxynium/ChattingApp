using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class RoleNotFoundException : DomainException
    {
        public RoleId RoleId { get; }
        public override string Code => "role_not_found";

        public RoleNotFoundException(RoleId roleId) : base($"Role[id={roleId}] was not found.")
        {
            RoleId = roleId;
        }
    }
}