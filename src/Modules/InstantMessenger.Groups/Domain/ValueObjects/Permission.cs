using Ardalis.SmartEnum;
using InstantMessenger.Groups.Domain.Exceptions;

namespace InstantMessenger.Groups.Domain.ValueObjects
{
    public class Permission : SmartEnum<Permission,int>
    {
        public static readonly Permission Administrator = new Permission(nameof(Administrator), 0x1);
        public static readonly Permission ManageGroup = new Permission(nameof(ManageGroup), 0x2);
        public static readonly Permission ManageRoles = new Permission(nameof(ManageRoles), 0x4);
        public static readonly Permission ManageChannels = new Permission(nameof(ManageChannels), 0x8);
        public static readonly Permission Kick = new Permission(nameof(Kick), 0x10);
        //public static readonly Permission Ban = new Permission(nameof(Ban), 0x20);
        public static readonly Permission ManageInvitations = new Permission(nameof(ManageInvitations), 0x40);
        //public static readonly Permission ChangeNickname = new Permission(nameof(ChangeNickname), 0x80);
        //public static readonly Permission ManageNicknames = new Permission(nameof(ManageNicknames), 0x100);
        //public static readonly Permission AttachFiles = new Permission(nameof(AttachFiles), 0x200);
        //public static readonly Permission EmbedLinks = new Permission(nameof(EmbedLinks), 0x400);
        //public static readonly Permission AddReactions = new Permission(nameof(AddReactions), 0x800);
        //public static readonly Permission MentionRoles = new Permission(nameof(MentionRoles), 0x1000);
        public static readonly Permission ReadMessages = new Permission(nameof(ReadMessages), 0x2000);
        public static readonly Permission SendMessages = new Permission(nameof(SendMessages), 0x4000);

        private Permission(string name, int value):base(name,value)
        {
        }

        public static Permission FromName(string name)
        {
            if (!TryFromName(name, out var permission))
            {
                throw new PermissionNotFoundException(name);
            }

            return permission;
        }
    }
}