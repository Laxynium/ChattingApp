namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class PermissionNotFoundException : DomainException
    {
        public override string Code => "permission_not_found";

        public PermissionNotFoundException(string name) : base($"Permission[name={name}] was not found.")
        {
        }
    }
}