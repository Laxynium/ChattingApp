namespace InstantMessenger.Groups.Domain.Entities.PermissionsVerification.Actions
{
    public abstract partial class Action
    {
        public class DeleteGroup : Action
        {
            public override string Name => nameof(DeleteGroup);
            public override bool CanExecute()
            {
                return AsMember.IsOwner;
            }

            public DeleteGroup(Member member):base(member)
            {
            }
        }
    }
}