using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Entities.PermissionsVerification.Actions
{
    public abstract partial class Action
    {
        public class GenerateInvitation : Action
        {
            public Permissions MemberPermissions { get; }
            public override string Name => nameof(GenerateInvitation);

            public GenerateInvitation(Member asMember, Permissions memberPermissions) : base(asMember)
            {
                MemberPermissions = memberPermissions;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;
                return MemberPermissions.Has(Permission.Administrator, Permission.Invite);
            }
        }

        public class RevokeInvitation : Action
        {
            public override string Name => nameof(RevokeInvitation);
            public Permissions MemberPermissions { get; }

            public RevokeInvitation(Member asMember, Permissions memberPermissions) : base(asMember)
            {
                MemberPermissions = memberPermissions;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;
                return MemberPermissions.Has(Permission.Administrator, Permission.Invite);
            }
        }
    }
}