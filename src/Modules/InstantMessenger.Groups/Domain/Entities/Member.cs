using System;
using System.Collections.Generic;
using System.Linq;
using Ardalis.SmartEnum;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.ValueObjects;
using NodaTime;

namespace InstantMessenger.Groups.Domain.Entities
{
    public class Member : Entity<MemberId>
    {
        private readonly HashSet<RoleId> _roles = new HashSet<RoleId>();
        public IEnumerable<RoleId> Roles => _roles.ToList();
        public UserId UserId { get; }//required in order to map user to member in particular group
        public MemberName Name { get; }
        public bool IsOwner { get; }
        public DateTimeOffset CreatedAt { get; }

        private Member(){}
        private Member(UserId userId, MemberId id, MemberName name, bool isOwner, RoleId role, DateTimeOffset createdAt): base(id)
        {
            UserId = userId;
            Name = name;
            IsOwner = isOwner;
            _roles.Add(role);
            CreatedAt = createdAt;
        }

        public static Member CreateOwner(UserId userId, MemberName name, Role everyoneRole, IClock clock)
        {
            return new Member(userId, MemberId.Create(),name, true, everyoneRole.Id,clock.GetCurrentInstant().InUtc().ToDateTimeOffset());
        }

        public static Member Create(UserId userId, MemberName name, Role everyoneRole, IClock clock)
        {
            return new Member(userId,MemberId.Create(),name, false, everyoneRole.Id, clock.GetCurrentInstant().InUtc().ToDateTimeOffset());
        }

        public void AddRole(Role role)
        {
            if (_roles.Contains(role.Id))
                return;
            _roles.Add(role.Id);
        }
    }

    public class Role : Entity<RoleId>
    {
        public RoleName Name { get; }
        public RolePriority Priority { get; private set; }
        public Permissions Permissions { get; private set; }

        public Role(RoleId id, RoleName name, RolePriority priority, Permissions permissions):base(id)
        {
            Name = name;
            Priority = priority;
            Permissions = permissions;
        }
        public static Role CreateEveryone()=>new Role(RoleId.Create(), RoleName.EveryOneRole, RolePriority.Lowest, Permissions.Empty());

        public static Role Create(RoleId id, RoleName name, RolePriority priority)
        {
            return new Role(id, name, priority, Permissions.Empty());
        }

        public void AddPermission(Permission permission)
        {
            Permissions = Permissions.Add(permission);
        }

        public void RemovePermission(Permission permission)
        {
            Permissions = Permissions.Remove(permission);
        }

        public void IncrementPriority()
        {
            if (this.Name == RoleName.EveryOneRole)
                return;
            Priority = Priority.Increased();
        }
    }

    public class Permission : SmartEnum<Permission,int>
    {
        public static readonly Permission Administrator = new Permission(nameof(Administrator), 0x1);
        public static readonly Permission ManageGroup = new Permission(nameof(ManageGroup), 0x2);
        public static readonly Permission ManageRoles = new Permission(nameof(ManageRoles), 0x4);
        public static readonly Permission ManageChannels = new Permission(nameof(ManageChannels), 0x8);
        public static readonly Permission Kick = new Permission(nameof(Kick), 0x10);
        public static readonly Permission Ban = new Permission(nameof(Ban), 0x20);
        public static readonly Permission Invite = new Permission(nameof(Invite), 0x40);
        public static readonly Permission ChangeNickname = new Permission(nameof(ChangeNickname), 0x80);
        public static readonly Permission ManageNicknames = new Permission(nameof(ManageNicknames), 0x100);
        public static readonly Permission AttachFiles = new Permission(nameof(AttachFiles), 0x200);
        public static readonly Permission EmbedLinks = new Permission(nameof(EmbedLinks), 0x400);
        public static readonly Permission AddReactions = new Permission(nameof(AddReactions), 0x800);
        public static readonly Permission MentionRoles = new Permission(nameof(MentionRoles), 0x1000);

        private Permission(string name, int value):base(name,value)
        {
        }
    }

    public class Permissions : ValueObject
    {
        public int Value { get; private set; }

        private Permissions(int value)
        {
            Value = value;
        }

        public static Permissions From(int value)=>new Permissions(value);

        public static Permissions Empty()=>new Permissions(0x0);

        public Permissions Add(Permission permission)
        {
            return new Permissions(Value | permission.Value);
        }

        public Permissions Remove(Permission permission)
        {
            return new Permissions(Value & ~permission.Value);
        }        
        public Permissions Add(Permissions permissions)
        {
            return new Permissions(Value | permissions.Value);
        }

        public Permissions Remove(Permissions permissions)
        {
            return new Permissions(Value & ~permissions.Value);
        }

        public bool Has(params Permission[] permissions) 
            => permissions.Aggregate(false, (current, permission) => current | (Value & permission.Value) == permission.Value);

        public IEnumerable<Permission> ToListOfPermissions() 
            => Permission.List.Where(p => this.Has(p)).ToArray();

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}