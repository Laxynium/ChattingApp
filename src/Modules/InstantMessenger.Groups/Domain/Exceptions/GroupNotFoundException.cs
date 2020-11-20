using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class GroupNotFoundException : DomainException
    {
        public override string Code => "group_not_found";

        public GroupNotFoundException(GroupId groupId) : base($"Group[{groupId} was not found.")
        {
        }
    }
    public class PermissionNotFoundException : DomainException
    {
        public override string Code => "permission_not_found";

        public PermissionNotFoundException(string name) : base($"Permission[name={name}] was not found.")
        {
        }
    }
}