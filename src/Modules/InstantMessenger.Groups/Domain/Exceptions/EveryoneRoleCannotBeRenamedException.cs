namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class EveryoneRoleCannotBeRenamedException : DomainException
    {
        public override string Code => "cannot_rename_everyone_role";

        public EveryoneRoleCannotBeRenamedException() : base($"Everyone role cannot be renamed")
        {
        }
    }
}