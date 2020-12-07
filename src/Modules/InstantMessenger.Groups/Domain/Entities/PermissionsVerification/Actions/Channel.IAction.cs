using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Entities.PermissionsVerification.Actions
{
    public abstract partial class Action
    {
        public class AddChannel : Action
        {
            public override string Name => nameof(AddChannel);
            public Permissions MemberPermissions { get; }


            public AddChannel(Member asMember, Permissions memberPermissions) : base(asMember)
            {
                MemberPermissions = memberPermissions;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;

                if (!MemberPermissions.Has(Permission.Administrator, Permission.ManageChannels))
                    return false;

                return true;
            }
        }

        public class RemoveChannel : Action
        {
            public Permissions MemberPermissions { get; }
            public Channel Channel { get; }
            public Role EveryoneRole { get; }

            public RemoveChannel(Member asMember,Permissions memberPermissions, Channel channel, Role everyoneRole) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                Channel = channel;
                EveryoneRole = everyoneRole;
            }

            public override string Name => nameof(RemoveChannel);
            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;
                var permissions = MemberPermissions;
                if (permissions.Has(Permission.Administrator))
                    return true;

                permissions = Channel.CalculatePermissions(permissions, AsMember, EveryoneRole.Id);


                if (permissions.Has(Permission.ManageChannels))
                    return true;

                return false;
            }
        }

        public class AllowPermissionForMember : Action
        {
            public override string Name => nameof(AllowPermissionForMember);
            public Permissions MemberPermissions { get; }
            public Channel Channel { get; }
            public Role EveryoneRole { get; }
            public Permission Permission { get; }


            public AllowPermissionForMember(Member asMember, Permissions memberPermissions, Channel channel, Role everyoneRole, Permission permission) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                Channel = channel;
                EveryoneRole = everyoneRole;
                Permission = permission;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;

                var memberPermissions = MemberPermissions;
                if (memberPermissions.Has(Permission.Administrator))
                    return true;

                if (Channel.CalculatePermissions(AsMember, EveryoneRole.Id).Has(Permission.ManageRoles))
                    return true;

                var calculatedPermissions = Channel.CalculatePermissions(memberPermissions, AsMember, EveryoneRole.Id);

                return calculatedPermissions.Has(Permission) && Permission != Permission.ManageRoles;
            }
        }

        public class DenyPermissionForMember : Action
        {
            public override string Name => nameof(DenyPermissionForMember);

            public Permissions MemberPermissions { get; }
            public Channel Channel { get; }
            public Role EveryoneRole { get; }
            public Permission Permission { get; }

            public DenyPermissionForMember(Member asMember, Permissions memberPermissions, Channel channel, Role everyoneRole, Permission permission) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                Channel = channel;
                EveryoneRole = everyoneRole;
                Permission = permission;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;

                var memberPermissions = MemberPermissions;
                if (memberPermissions.Has(Permission.Administrator))
                    return true;

                if (Channel.CalculatePermissions(AsMember, EveryoneRole.Id).Has(Permission.ManageRoles))
                    return true;

                var calculatedPermissions = Channel.CalculatePermissions(memberPermissions, AsMember, EveryoneRole.Id);

                return calculatedPermissions.Has(Permission) && Permission != Permission.ManageRoles;
            }
        }

        public class RemoveOverrideForMember : Action
        {
            public override string Name => nameof(RemoveOverrideForMember);

            public Permissions MemberPermissions { get; }
            public Channel Channel { get; }
            public Role EveryoneRole { get; }
            public Permission Permission { get; }

            public RemoveOverrideForMember(Member asMember, Permissions memberPermissions, Channel channel, Role everyoneRole, Permission permission) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                Channel = channel;
                EveryoneRole = everyoneRole;
                Permission = permission;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;

                var memberPermissions = MemberPermissions;
                if (memberPermissions.Has(Permission.Administrator))
                    return true;

                if (Channel.CalculatePermissions(AsMember, EveryoneRole.Id).Has(Permission.ManageRoles))
                    return true;

                var calculatedPermissions = Channel.CalculatePermissions(memberPermissions, AsMember, EveryoneRole.Id);

                return calculatedPermissions.Has(Permission) && Permission != Permission.ManageRoles;
            }
        }

        public class AllowPermissionForRole : Action
        {
            public override string Name => nameof(AllowPermissionForRole);

            public Permissions MemberPermissions { get; }
            public Channel Channel { get; }
            public Role EveryoneRole { get; }
            public Permission Permission { get; }

            public AllowPermissionForRole(Member asMember, Permissions memberPermissions, Channel channel, Role everyoneRole, Permission permission) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                Channel = channel;
                EveryoneRole = everyoneRole;
                Permission = permission;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;

                var memberPermissions = MemberPermissions;
                if (memberPermissions.Has(Permission.Administrator))
                    return true;

                if (Channel.CalculatePermissions(AsMember, EveryoneRole.Id).Has(Permission.ManageRoles))
                    return true;

                var calculatedPermissions = Channel.CalculatePermissions(memberPermissions, AsMember, EveryoneRole.Id);

                return calculatedPermissions.Has(Permission) && Permission != Permission.ManageRoles;
            }
        }

        public class DenyPermissionForRole : Action
        {
            public override string Name => nameof(DenyPermissionForRole);


            public Permissions MemberPermissions { get; }
            public Channel Channel { get; }
            public Role EveryoneRole { get; }
            public Permission Permission { get; }

            public DenyPermissionForRole(Member asMember, Permissions memberPermissions, Channel channel, Role everyoneRole, Permission permission) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                Channel = channel;
                EveryoneRole = everyoneRole;
                Permission = permission;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;

                var memberPermissions = MemberPermissions;
                if (memberPermissions.Has(Permission.Administrator))
                    return true;

                if (Channel.CalculatePermissions(AsMember, EveryoneRole.Id).Has(Permission.ManageRoles))
                    return true;

                var calculatedPermissions = Channel.CalculatePermissions(memberPermissions, AsMember, EveryoneRole.Id);

                return calculatedPermissions.Has(Permission) && Permission != Permission.ManageRoles;
            }
        }

        public class RemoveOverrideForRole : Action
        {
            public override string Name => nameof(RemoveOverrideForRole);


            public Permissions MemberPermissions { get; }
            public Channel Channel { get; }
            public Role EveryoneRole { get; }
            public Permission Permission { get; }

            public RemoveOverrideForRole(Member asMember, Permissions memberPermissions, Channel channel, Role everyoneRole, Permission permission) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                Channel = channel;
                EveryoneRole = everyoneRole;
                Permission = permission;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;

                var memberPermissions = MemberPermissions;
                if (memberPermissions.Has(Permission.Administrator))
                    return true;

                if (Channel.CalculatePermissions(AsMember, EveryoneRole.Id).Has(Permission.ManageRoles))
                    return true;

                var calculatedPermissions = Channel.CalculatePermissions(memberPermissions, AsMember, EveryoneRole.Id);

                return calculatedPermissions.Has(Permission) && Permission != Permission.ManageRoles;
            }
        }
    }
}