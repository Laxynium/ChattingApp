//using System;
//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Linq;
//using Ardalis.SmartEnum;
//using CSharpFunctionalExtensions;

//namespace InstantMessenger.Groups.Domain
//{
//    public abstract class Action : ValueObject
//    {
//        public string Name { get; }

//        protected Action(string name)
//        {
//            Name = name;
//        }
//    }

//    public class AddRole : Action
//    {
//        public Role Role { get; }

//        public AddRole(Role role) : base(nameof(AddRole))
//        {
//            Role = role;
//        }
//        protected override IEnumerable<object> GetEqualityComponents()
//        {
//            yield return Name;
//        }
//    }
//    public class RemoveRole : Action
//    {
//        public RemoveRole() : base(nameof(RemoveRole))
//        {
//        }
//        protected override IEnumerable<object> GetEqualityComponents()
//        {
//            yield return Name;
//        }
//    }
//    public class ChangeRoleName : Action
//    {
//        public ChangeRoleName() : base(nameof(ChangeRoleName))
//        {
//        }
//        protected override IEnumerable<object> GetEqualityComponents()
//        {
//            yield return Name;
//        }
//    }

//    public class PermissionVerifier
//    {
//        public bool Verify(Member member, Action action)
//        {
//            var result = action switch
//            {
//                AddRole x => HandleAddRole(member,x),
//                _ => false
//            };
//            return result;
//        }

//        private bool HandleAddRole(Member member, AddRole action)
//        {
//            var roleWithAdministratorPermission = member.GetHighestRoleWith(PermissionXyz.Administrator);
//            if (roleWithAdministratorPermission.HasValue)
//            {
//                return action.Role < roleWithAdministratorPermission.Value;
//            }

//            var roleWithManageRolesPermission = member.GetHighestRoleWith(PermissionXyz.ManageRoles);
//            if (roleWithManageRolesPermission.HasValue)
//            {
//                return action.Role < roleWithManageRolesPermission.Value;
//            }

//            return false;
//        }
//    }
//    public class Member: Entity<Guid>
//    {
//        private readonly ISet<Role> _roles;

//        private Member(ISet<Role> roles)
//        {
//            _roles = roles ?? new HashSet<Role>();
//        }

//        public ImmutableHashSet<PermissionXyz> Permissions => _roles.SelectMany(x => x.Permissions).ToImmutableHashSet();

//        public static Member Create(ISet<Role> roles)
//        {
//            return new Member(roles);
//        }

//        public Maybe<Role> GetHighestRoleWith(PermissionXyz permission)
//        {
//            return _roles.Where(x => x.Permissions.Contains(permission))
//                .OrderBy(x => x.Priority)
//                .TryFirst();
//        }
//    }

//    public enum PermissionType
//    {
//        Administrator,
//        ManageRoles
//    }

//    public class RolePriority : SimpleValueObject<int>
//    {
//        public static readonly RolePriority Highest = new RolePriority(0);
//        public static readonly RolePriority Lowest = new RolePriority(-1);

//        private RolePriority(int value):base(value)
//        {
//        }

//        public static bool operator <(RolePriority a, RolePriority b)
//        {
//            if (a == Lowest)
//                return b != Lowest;
//            return a.Value < b.Value;
//        }

//        public static bool operator >(RolePriority a, RolePriority b)
//        {
//            if (b == Lowest)
//                return a != Lowest;
//            return a.Value > b.Value;
//        }

//        public static RolePriority Create(int value)
//        {
//            if(value <=0)
//                throw new ArgumentException($"User defined role priority must be greater than 0");
//            return new RolePriority(value);
//        }
//    }
//    public class Role
//    {
//        private readonly ISet<PermissionXyz> _permissions;

//        public string Name { get; }

//        public RolePriority Priority { get; }

//        public static readonly Role Owner = new Role(nameof(Owner), RolePriority.Highest);

//        public static readonly Role Everyone = new Role(nameof(Everyone),RolePriority.Lowest);

//        private Role(string name, RolePriority priority, ISet<PermissionXyz> permissions = null)
//        {
//            Name = name;
//            Priority = priority;
//            _permissions = permissions ?? new HashSet<PermissionXyz>();
//        }

//        public static Role Create(string name, RolePriority priority, ISet<PermissionXyz> permissions = null)
//        {
//            if(priority == RolePriority.Lowest)
//                throw new ArgumentException($"User defined role cannot have the lowest priority");
//            if(priority == RolePriority.Highest)
//                throw new ArgumentException($"User defined role cannot have the highest priority");
//            return new Role(name, priority, permissions);
//        }

//        public ImmutableHashSet<PermissionXyz> Permissions => _permissions.ToImmutableHashSet();

//        public static bool operator < (Role a, Role b) 
//            => a.Priority < b.Priority;

//        public static bool operator > (Role a, Role b) 
//            => a.Priority > b.Priority;
//    }

//    public class PermissionXyz : SmartEnum<PermissionXyz>
//    {
//        public static readonly PermissionXyz Administrator = new PermissionXyz(nameof(Administrator),1);
//        public static readonly PermissionXyz ManageGroup = new PermissionXyz(nameof(ManageGroup),2);
//        public static readonly PermissionXyz ManageRoles = new PermissionXyz(nameof(ManageRoles),3);
//        public static readonly PermissionXyz ManageChannels = new PermissionXyz(nameof(ManageChannels),4);
//        public static readonly PermissionXyz Kick = new PermissionXyz(nameof(Kick),5);
//        public static readonly PermissionXyz Ban = new PermissionXyz(nameof(Ban),6);
//        public static readonly PermissionXyz Invite = new PermissionXyz(nameof(Invite),7);
//        public static readonly PermissionXyz ChangeNickname = new PermissionXyz(nameof(ChangeNickname),8);
//        public static readonly PermissionXyz ManageNicknames = new PermissionXyz(nameof(ManageNicknames),10);
//        public static readonly PermissionXyz AttachFiles = new PermissionXyz(nameof(AttachFiles),11);
//        public static readonly PermissionXyz EmbedLinks = new PermissionXyz(nameof(EmbedLinks),12);
//        public static readonly PermissionXyz AddReactions = new PermissionXyz(nameof(AddReactions),13);
//        public static readonly PermissionXyz MentionRoles = new PermissionXyz(nameof(MentionRoles),14);

//        public PermissionXyz(string name, int value) : base(name, value)
//        {
//        }
//    }
//}