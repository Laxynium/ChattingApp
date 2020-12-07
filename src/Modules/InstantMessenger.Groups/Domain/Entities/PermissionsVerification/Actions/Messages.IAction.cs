using InstantMessenger.Groups.Domain.ValueObjects;

namespace InstantMessenger.Groups.Domain.Entities.PermissionsVerification.Actions
{
    public abstract partial class Action
    {
        public class SendMessage : Action
        {
            public Permissions MemberPermissions { get; }
            public Channel Channel { get; }
            public Role EveryoneRole { get; }
            public override string Name => nameof(SendMessage);

            public SendMessage(Member asMember, Permissions memberPermissions,Channel channel, Role everyoneRole) : base(asMember)
            {
                MemberPermissions = memberPermissions;
                Channel = channel;
                EveryoneRole = everyoneRole;
            }

            public override bool CanExecute()
            {
                if (AsMember.IsOwner)
                    return true;
                var permissions = MemberPermissions;
                if (permissions.Has(Permission.Administrator))
                    return true;

                permissions = Channel.CalculatePermissions(permissions, AsMember, EveryoneRole.Id);

                if (permissions.Has(Permission.SendMessages))
                    return true;

                return false;
            }
        }
    }
}