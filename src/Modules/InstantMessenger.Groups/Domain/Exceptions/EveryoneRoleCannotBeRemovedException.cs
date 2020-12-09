namespace InstantMessenger.Groups.Domain.Exceptions
{
    public class EveryoneRoleCannotBeRemovedException : DomainException
    {
        public override string Code => "cannot_remove_everyone_role";

        public EveryoneRoleCannotBeRemovedException() : base($"Everyone role cannot be removed")
        {
        }
    }
}