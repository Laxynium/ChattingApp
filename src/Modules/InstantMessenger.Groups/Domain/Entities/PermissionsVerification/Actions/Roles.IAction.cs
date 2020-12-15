using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Entities.PermissionsVerification.Actions
{
    public abstract partial class Action
    {
        public class AddPermissionToRole : Action
        {
            public override string Name => nameof(AddPermissionToRole);
            public Permissions MemberPermissions { get; }
            public IEnumerable<Role> MemberRoles { get; }
            public Role AddedRole { get; }
            public Permission AddedPermission { get; }

            public AddPermissionToRole(Member asMember,
                Permissions memberPermissions,
                IEnumerable<Role> memberRoles,
                Role addedRole,
                Permission addedPermission) : base(
                asMember
            )
            {
                MemberPermissions = memberPermissions;
                MemberRoles = memberRoles;
                AddedRole = addedRole;
                AddedPermission = addedPermission;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;

                if (!MemberPermissions.Has(Permission.Administrator, Permission.ManageRoles))
                    return false;

                var memberHighestRole = GetRoleWithHighestPriority(MemberRoles);
                if (AddedRole.Priority >= memberHighestRole.Priority)
                    return false;

                if (!MemberPermissions.Has(Permission.Administrator) && !MemberPermissions.Has(AddedPermission))
                    return false;

                return true;
            }
        }

        public class RemovePermissionFromRole : Action
        {
            public override string Name => nameof(RemovePermissionFromRole);

            public Permissions MemberPermissions { get; }
            public IEnumerable<Role> MemberRoles { get; }
            public Role AddedRole { get; }
            public Permission AddedPermission { get; }

            public RemovePermissionFromRole(Member asMember,
                Permissions memberPermissions,
                IEnumerable<Role> memberRoles,
                Role addedRole,
                Permission addedPermission) : base(
                asMember
            )
            {
                MemberPermissions = memberPermissions;
                MemberRoles = memberRoles;
                AddedRole = addedRole;
                AddedPermission = addedPermission;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;

                if (!MemberPermissions.Has(Permission.Administrator, Permission.ManageRoles))
                    return false;

                var memberHighestRole = GetRoleWithHighestPriority(MemberRoles);
                if (AddedRole.Priority >= memberHighestRole.Priority)
                    return false;

                if (!MemberPermissions.Has(Permission.Administrator) && !MemberPermissions.Has(AddedPermission))
                    return false;

                return true;
            }
        }

        public class AddRole : Action
        {
            public Permissions MemberPermissions { get; }
            public override string Name => nameof(AddRole);

            public AddRole(Member asMember, Permissions memberPermissions) : base(asMember)
            {
                MemberPermissions = memberPermissions;
            }

            public override bool CanExecute() => 
                AsMember.IsOwner || MemberPermissions.Has(Permission.Administrator, Permission.ManageRoles);
        }

        public class RemoveRole : Action
        {
            public Permissions MemberPermissions { get; }
            public IEnumerable<Role> MemberRoles { get; }
            public Role RoleBeingRemoved { get; }
            public override string Name => nameof(RemoveRole);

            public RemoveRole(Member asMember, Permissions memberPermissions, IEnumerable<Role> memberRoles, Role roleBeingRemoved) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                MemberRoles = memberRoles;
                RoleBeingRemoved = roleBeingRemoved;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;
                if (!MemberPermissions.Has(Permission.Administrator, Permission.ManageRoles))
                    return false;

                var memberHighestRole = GetRoleWithHighestPriority(MemberRoles);
                
                if (RoleBeingRemoved.Priority >= memberHighestRole.Priority)
                    return false;

                return true;
            }
        }

        public class RenameRole : Action
        {
            public Permissions MemberPermissions { get; }
            public IEnumerable<Role> MemberRoles { get; }
            public Role RoleBeingRenamed { get; }
            public override string Name => nameof(RenameRole);

            public RenameRole(Member asMember, Permissions memberPermissions, IEnumerable<Role> memberRoles, Role roleBeingRenamed) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                MemberRoles = memberRoles;
                RoleBeingRenamed = roleBeingRenamed;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;
                if (!MemberPermissions.Has(Permission.Administrator, Permission.ManageRoles))
                    return false;

                var memberHighestRole = GetRoleWithHighestPriority(MemberRoles);

                if (RoleBeingRenamed.Priority >= memberHighestRole.Priority)
                    return false;

                return true;
            }


        }


        public class MoveDownRoleInHierarchy : Action
        {
            public Permissions MemberPermissions { get; }
            public IEnumerable<Role> MemberRoles { get; }
            public IEnumerable<Role> UserDefinedRoles { get; }
            public Role MovedRole { get; }
            public override string Name => nameof(MoveDownRoleInHierarchy);

            public MoveDownRoleInHierarchy(Member asMember, Permissions memberPermissions,IEnumerable<Role> memberRoles, IEnumerable<Role> userDefinedRoles,Role movedRole) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                MemberRoles = memberRoles;
                UserDefinedRoles = userDefinedRoles;
                MovedRole = movedRole;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;

                if (!MemberPermissions.Has(Permission.Administrator, Permission.ManageRoles))
                    return false;

                var memberHighestRole = GetRoleWithHighestPriority(MemberRoles);
                if (MovedRole.Priority >= memberHighestRole.Priority)
                    return false;

                return true;
            }
        }

        public class MoveUpRoleInHierarchy : Action
        {
            public Permissions MemberPermissions { get; }
            public IEnumerable<Role> MemberRoles { get; }
            public IEnumerable<Role> UserDefinedRoles { get; }
            public Role MovedRole { get; }
            public override string Name => nameof(MoveUpRoleInHierarchy);

            public MoveUpRoleInHierarchy(Member asMember, Permissions memberPermissions, IEnumerable<Role> memberRoles, IEnumerable<Role> userDefinedRoles, Role movedRole) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                MemberRoles = memberRoles;
                UserDefinedRoles = userDefinedRoles;
                MovedRole = movedRole;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;

                if (!MemberPermissions.Has(Permission.Administrator, Permission.ManageRoles))
                    return false;

                var memberHighestRole = GetRoleWithHighestPriority(MemberRoles);
                if (MovedRole.Priority >= memberHighestRole.Priority)
                    return false;

                return true;
            }
        }

        private static Role GetRoleWithHighestPriority(IEnumerable<Role> memberRoles)
        {
            var highestRole = memberRoles.OrderByDescending(x => x.Priority).First();
            return highestRole;
        }
    }
}