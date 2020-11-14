using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;

namespace InstantMessenger.Groups.Domain
{
    public class GroupId : SimpleValueObject<Guid>
    {
        private GroupId():base(default){}
        private GroupId(Guid value) : base(value)
        {
        }
        public GroupId Create() => new GroupId(Guid.NewGuid());
        public GroupId From(Guid id) => new GroupId(id);
        
    }

    public class Group : Entity<Guid>
    {
        public string Name { get; }

        private Group(){}
        public Group(Guid id, string name):base(id)
        {
            Name = name;
        }
    }

    public class Priority : SimpleValueObject<int>
    {
        public Priority(int value) : base(value)
        {
        }
    }

    public class GroupMember : Entity<Guid>
    {
        private ISet<Role> _roles;

        public GroupMember(ISet<Role> roles)
        {
            _roles = roles;
        }
    }

    public class Channel : Entity<Guid>
    {
        private ISet<Role> _roles = new HashSet<Role>(); // dynamic 
        private ISet<RoleOverride> _roleOverrides = new HashSet<RoleOverride>();
        private ISet<GroupMember> _groupMembers;
    }

    public class ChannelSpecificMemberPermissionOverride
    {

    }

    public class RoleOverride
    {

    }

    public class Role : Entity<Guid>
    {
        public static readonly Role Everyone = new Role(new HashSet<Permission>
        {
            Permission.ManageRoles,
            Permission.ManageServer
        });
        public static readonly Role Administrator = new Role(new HashSet<Permission>
        {
            Permission.Administrator
        });
        private ISet<Permission> _permissions;

        public Role(ISet<Permission> permissions)
        {
            _permissions = permissions;
        }
    }



    public class Permission : Entity<Guid>
    {
        public static readonly Permission Administrator = new Permission("Administrator", new Priority(1));
        public static readonly Permission ManageServer = new Permission("ManageServer", new Priority(1));
        public static readonly Permission ManageRoles = new Permission("MangeRoles", new Priority(1));
        public string Name { get; }
        public Priority Priority { get; }

        public Permission(string name, Priority priority)
        {
            Name = name;
            Priority = priority;
        }
    }
}