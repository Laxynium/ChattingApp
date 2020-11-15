using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using NodaTime;

namespace InstantMessenger.Groups.Domain
{
    public class GroupId : SimpleValueObject<Guid>
    {
        private GroupId():base(default){}
        private GroupId(Guid value) : base(value)
        {
        }
        public static GroupId Create() => new GroupId(Guid.NewGuid());
        public static GroupId From(Guid id) => new GroupId(id);
        
    }

    public class GroupName : SimpleValueObject<string>
    {
        private GroupName():base(default){}
        private GroupName(string value) : base(value)
        {
        }

        public static GroupName Create(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentException($"Group name cannot be null or whitespace");
            return new GroupName(value);
        }
    }
    
    public class Group : Entity<GroupId>
    {
        private readonly HashSet<Member> _members = new HashSet<Member>();
        public IEnumerable<Member> Members => _members.ToList();
        public GroupName Name { get; }
        public DateTimeOffset CreatedAt { get; }

        private Group(){}

        private Group(GroupId id, GroupName name, DateTimeOffset createdAt):base(id)
        {
            Name = name;
            CreatedAt = createdAt;
        }

        public static Group Create(GroupId id, GroupName name, UserId userId, MemberId ownerId, MemberName ownerName, IClock clock)
        {
            var owner = Member.CreateOwner(userId,ownerId, ownerName, clock);
            var group = new Group(id, name, clock.GetCurrentInstant().InUtc().ToDateTimeOffset());
            group.WithOwner(owner);
            return group;
        }

        public void AddMember(Member member)
        {
            if (member.IsOwner)
                throw new ArgumentException("Group can have only one owner");

            if (_members.Contains(member))
                throw new ArgumentException($"Given member is already in group");

            _members.Add(member);
        }

        private void WithOwner(Member owner)
        {
            _members.Add(owner);
        }
    }

    public class UserId : SimpleValueObject<Guid>
    {
        private UserId() : base(default) { }
        private UserId(Guid value) : base(value)
        {
        }
        public static UserId Create() => new UserId(Guid.NewGuid());
        public static UserId From(Guid id) => new UserId(id);

    }
    public class MemberId : SimpleValueObject<Guid>
    {
        private MemberId() : base(default) { }
        private MemberId(Guid value) : base(value)
        {
        }
        public static MemberId Create() => new MemberId(Guid.NewGuid());
        public static MemberId From(Guid id) => new MemberId(id);

    }

    public class MemberName : SimpleValueObject<string>
    {
        private MemberName():base(default){}
        private MemberName(string value) : base(value)
        {
        }
        public static MemberName Create(string value)
        {
            if(string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Member name cannot be null or whitespace");
            return new MemberName(value);
        }
    }

    public class Member : Entity<MemberId>
    {
        public UserId UserId { get; }//required in order to map user to member in particular group
        public MemberName Name { get; }
        public bool IsOwner { get; }
        public DateTimeOffset CreatedAt { get; }

        private Member(){}
        private Member(UserId userId, MemberId id, MemberName name, bool isOwner, DateTimeOffset createdAt): base(id)
        {
            UserId = userId;
            Name = name;
            IsOwner = isOwner;
            CreatedAt = createdAt;
        }

        public static Member CreateOwner(UserId userId, MemberId id, MemberName name, IClock clock)
        {
            return new Member(userId, id,name, true, clock.GetCurrentInstant().InUtc().ToDateTimeOffset());
        }

        public static Member Create(UserId userId, MemberId id, MemberName name, IClock clock)
        {
            return new Member(userId,id,name, false, clock.GetCurrentInstant().InUtc().ToDateTimeOffset());
        }
    }

    public interface IOnlyOneOwnerRule
    {
        //if (!await rule.Verify())
        //{
        //    throw new InvalidOperationException("There can be only one owner of group");
        //}

    Task<bool> Verify();
    }

    //public class ServerAction : SmartEnum<ServerAction,string>
    //{
    //    public Permission Permission { get; }
    //    public static readonly ServerAction SendMessage = new ServerAction("Send Message", "send_message", Permission.ManageRoles);
    //    public static readonly ServerAction Mention = new ServerAction("Mention", "mention", Permission.ManageRoles);
    //    public static readonly ServerAction ManageRoles = new ServerAction("Manage Roles", "manage_roles", Permission.ManageRoles);
    //    public static readonly ServerAction ManageChannels = new ServerAction("Manage Channels", "manage_channel", Permission.ManageRoles);
    //    public ServerAction(string name, string value, Permission permission) : base(name, value)
    //    {
    //        Permission = permission;
    //    }
    //}

    //public class PermissionVerifier
    //{
    //    public bool Verify(GroupMember member, ServerAction action)
    //    {
    //        GetPermissions(member);
    //        if (action.Permission == Permission.ManageChannels)
    //        {
    //            var channelId = Guid.NewGuid();
    //            GetOverrides(member, channelId);
    //        }
    //        return true;
    //    }

    //    private void GetOverrides(GroupMember member, Guid channelId)
    //    {
    //        //read user specific permissions
    //        //read user roles specific permissions
    //    }

    //    private void GetPermissions(GroupMember member)
    //    {
    //        //read user roles specific permissions
    //    }
    //}

    //public class Priority : SimpleValueObject<int>
    //{
    //    public Priority(int value) : base(value)
    //    {
    //    }
    //}

    //public class GroupMember : Entity<Guid>
    //{
    //    private ISet<Role> _roles;

    //    public GroupMember(ISet<Role> roles)
    //    {
    //        _roles = roles;
    //    }
    //}

    //public class Channel : Entity<Guid>
    //{
    //    private ISet<Role> _roles = new HashSet<Role>(); // dynamic 
    //    private ISet<RoleOverride> _roleOverrides = new HashSet<RoleOverride>();
    //    private ISet<GroupMember> _groupMembers;
    //}

    //public class ChannelSpecificMemberPermissionOverride
    //{

    //}

    //public class RoleOverride
    //{

    //}

    //public class Role : Entity<Guid>
    //{
    //    public static readonly Role Everyone = new Role(new HashSet<Permission>
    //    {
    //        Permission.ManageRoles,
    //        Permission.ManageServer
    //    });
    //    public static readonly Role Administrator = new Role(new HashSet<Permission>
    //    {
    //        Permission.Administrator
    //    });
    //    private ISet<Permission> _permissions;

    //    public Role(ISet<Permission> permissions)
    //    {
    //        _permissions = permissions;
    //    }
    //}



    //public class Permission : Entity<Guid>
    //{
    //    public static readonly Permission Administrator = new Permission("Administrator", new Priority(1));
    //    public static readonly Permission ManageServer = new Permission("ManageServer", new Priority(1));
    //    public static readonly Permission ManageRoles = new Permission("MangeRoles", new Priority(1));
    //    public static readonly Permission ManageChannels = new Permission("MangeRoles", new Priority(1));
    //    public string Name { get; }
    //    public Priority Priority { get; }

    //    public Permission(string name, Priority priority)
    //    {
    //        Name = name;
    //        Priority = priority;
    //    }
    //}
}