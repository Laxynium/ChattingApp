using System.Collections.Generic;
using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Entities.PermissionsVerification.Actions
{
    public abstract partial class Action
    {
        public class AssignRole : Action
        {
            public override string Name => nameof(AssignRole);
            public Permissions MemberPermissions { get; }
            public IEnumerable<Role> MemberRoles { get; }
            public IEnumerable<Role> UserDefinedRoles { get; }
            public Role AssignedRole { get; }

            public AssignRole(Member asMember, Permissions memberPermissions, IEnumerable<Role> memberRoles, IEnumerable<Role> userDefinedRoles, Role assignedRole) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                MemberRoles = memberRoles;
                UserDefinedRoles = userDefinedRoles;
                AssignedRole = assignedRole;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;
                if (!MemberPermissions.Has(Permission.Administrator, Permission.ManageRoles))
                    return false;

                var highestRole = GetRoleWithHighestPriority(MemberRoles);
                return highestRole.Priority > AssignedRole.Priority;
            }
        }


        public class RemoveRoleFromMember : Action
        {
            public override string Name => nameof(RemoveRoleFromMember);
            public Permissions MemberPermissions { get; }
            public IEnumerable<Role> MemberRoles { get; }
            public IEnumerable<Role> UserDefinedRoles { get; }
            public Role AssignedRole { get; }

            public RemoveRoleFromMember(Member asMember, Permissions memberPermissions, IEnumerable<Role> memberRoles, IEnumerable<Role> userDefinedRoles, Role assignedRole) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                MemberRoles = memberRoles;
                UserDefinedRoles = userDefinedRoles;
                AssignedRole = assignedRole;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;
                if (!MemberPermissions.Has(Permission.Administrator, Permission.ManageRoles))
                    return false;

                var highestRole = GetRoleWithHighestPriority(MemberRoles);
                return highestRole.Priority > AssignedRole.Priority;
            }

        }

        public class KickMember : Action
        {
            public override string Name => nameof(KickMember);
            public Permissions AsMemberPermissions { get; }
            public IEnumerable<Role> AsMemberRoles { get; }
            public Member OnMember { get; }
            public IEnumerable<Role> OnMemberRoles { get; }

            public KickMember(Member asMember,
                Permissions asMemberPermissions,
                IEnumerable<Role> asMemberRoles,
                Member onMember,
                IEnumerable<Role> onMemberRoles
                ) : base(
                asMember
            )
            {
                AsMemberPermissions = asMemberPermissions;
                AsMemberRoles = asMemberRoles;
                OnMember = onMember;
                OnMemberRoles = onMemberRoles;
            }

            public override bool CanExecute()
            {
                if (OnMember.IsOwner)
                    return false;

                if (AsMember == OnMember)
                    return false;

                if (AsMember.IsOwner)
                    return true;

                if (!AsMemberPermissions.Has(Permission.Administrator, Permission.Kick))
                    return false;

                var asMemberHighestRole = GetRoleWithHighestPriority(AsMemberRoles);
                var onMemberHighestRole = GetRoleWithHighestPriority(OnMemberRoles);

                if (asMemberHighestRole.Priority <= onMemberHighestRole.Priority)
                    return false;

                return true;
            }
        }
    }
}