using InstantMessenger.Groups.Domain.ValueObjects;

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
        public class RenameGroup : Action
        {
            public Permissions MemberPermissions { get; }
            public override string Name => nameof(DeleteGroup);

            public RenameGroup(Member member, Permissions memberPermissions):base(member)
            {
                MemberPermissions = memberPermissions;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;
                if (MemberPermissions.Has(Permission.Administrator))
                    return true;
                return MemberPermissions.Has(Permission.ManageGroup);
            }
        }
    }
}