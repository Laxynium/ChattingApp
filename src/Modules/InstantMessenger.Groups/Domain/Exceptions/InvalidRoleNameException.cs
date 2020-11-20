namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class InvalidRoleNameException : DomainException
    {
        public override string Code => "invalid_role_name";

        public InvalidRoleNameException(string name):base($"'{name}' is invalid role name.")
        {
        }
    }
}