using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstantMessenger.Groups;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Features.Members.Add;
using InstantMessenger.Groups.Api.Features.Members.AssignRole;
using InstantMessenger.Groups.Api.Features.Roles.AddPermissionToRole;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;

namespace InstantMessenger.UnitTests.Common
{

    internal class GroupBuilder
    {
        private readonly GroupsModuleFacade _facade;
        private GroupDto _group;
        private readonly List<Func<Task>> _buildActions = new List<Func<Task>>();
        public GroupBuilder(GroupsModuleFacade facade)
        {
            _facade = facade;
        }

        public static GroupBuilder For(GroupsModuleFacade facade) => new GroupBuilder(facade);

        public GroupBuilder CreateGroup(string groupName)
        {
            _group = new GroupDto(groupName);
            _buildActions.Add(async ()=>await _facade.CreateGroup(_group.OwnerId, _group.GroupId, _group.GroupName));
            return this;
        }

        public AsUserBuilder AsOwner() => new AsUserBuilder(this,_group.OwnerId);

        public AsUserBuilder AsMember(int memberNumber) => new AsUserBuilder(this, _group.Member(memberNumber).UserId);

        public async Task<GroupDto> Build()
        {
            foreach (var buildAction in _buildActions) 
                await buildAction.Invoke();
            return _group;
        }

        private void AddRole(RoleDto role) => _group.AddRole(role);

        private void AddMember(MemberDto member) => _group.AddMember(member);

        private void AddBuildAction(Func<Task> action) => _buildActions.Add(action);

        internal class AsUserBuilder
        {
            private readonly GroupBuilder _builder;
            private readonly GroupsModuleFacade _facade;
            private readonly Guid _userIdContext;
            private readonly GroupDto _group;

            public AsUserBuilder(GroupBuilder builder, Guid userIdContext)
            {
                _builder = builder;
                _facade = builder._facade;
                _group = _builder._group;
                _userIdContext = userIdContext;
            }

            public RoleBuilder CreateRole(string roleName)
            {
                var role = new RoleDto(roleName);
                _builder.AddRole(role);
                _builder.AddBuildAction(async () => await _facade.AddRole(_userIdContext, _group.GroupId, role.RoleId, role.Name));
                return new RoleBuilder(this, role);
            }
            public MemberBuilder CreateMember()
            {
                var member = new MemberDto();
                _builder.AddMember(member);
                _builder.AddBuildAction(async () => await _facade.AddMember(_group.GroupId, member.UserId));
                return new MemberBuilder(this,member);
            }

            public GroupBuilder Build() => _builder;
            private void AddBuildAction(Func<Task> action) => _builder.AddBuildAction(action);

            internal class RoleBuilder
            {
                private readonly AsUserBuilder _builder;
                private readonly RoleDto _role;
                private readonly GroupDto _group;
                private readonly GroupsModuleFacade _groupsModuleFacade;

                public RoleBuilder(AsUserBuilder builder, RoleDto role)
                {
                    _builder = builder;
                    _role = role;
                    _group = _builder._builder._group;
                    _groupsModuleFacade = _builder._facade;
                }

                public RoleBuilder AddPermission(string name)
                {
                    _builder.AddBuildAction(async () => await _groupsModuleFacade.AddPermission(_builder._userIdContext, _group.GroupId, _role.RoleId, name));
                    return this;
                }

                public AsUserBuilder Build() => _builder;
            }

            internal class MemberBuilder
            {
                private readonly AsUserBuilder _builder;
                private readonly MemberDto _memberContext;
                private readonly GroupsModuleFacade _facade;
                private readonly GroupDto _group;

                public MemberBuilder(AsUserBuilder builder, MemberDto memberContext)
                {
                    _builder = builder;
                    _memberContext = memberContext;
                    _facade = _builder._facade;
                    _group = _builder._builder._group;
                }
                public MemberBuilder AssignRole(int roleNumber)
                {
                    var role = _group.Role(roleNumber);
                    _builder.AddBuildAction(async () => await _facade.AssignRole(_builder._userIdContext, _group.GroupId, _memberContext.UserId, role.RoleId));
                    return this;
                }

                public AsUserBuilder Build() => _builder;
            }
        }


        internal class GroupDto
        {
            private readonly List<RoleDto> _roles = new List<RoleDto>();
            private readonly List<MemberDto> _members = new List<MemberDto>();
            internal Guid OwnerId { get; }
            internal Guid GroupId { get; }
            internal string GroupName { get; }

            internal GroupDto(string groupName)
            {
                OwnerId = Guid.NewGuid();
                GroupId = Guid.NewGuid();
                GroupName = groupName;
            }

            internal RoleDto Role(int i) => _roles[i-1];
            internal MemberDto Member(int i) => _members[i-1];
            internal GroupDto AddRole(RoleDto role)
            {
                _roles.Add(role);
                return this;
            }
            internal GroupDto AddMember(MemberDto member)
            {
                _members.Add(member);
                return this;
            }
        }


        internal class RoleDto
        {
            internal Guid RoleId { get; }
            internal string Name { get; }
            internal RoleDto(string name)
            {
                RoleId = Guid.NewGuid();
                Name = name;
            }
        }
        internal class MemberDto
        {
            internal Guid UserId { get; }

            internal MemberDto()
            {
                UserId = Guid.NewGuid();
            }
        }
    }

    internal static class GroupModuleFacadeExtensions
    {
        internal static async Task CreateGroup(this GroupsModuleFacade facade, 
            Guid userId, 
            Guid groupId, 
            string groupName)
            => await facade.SendAsync(new CreateGroupCommand(userId, groupId, groupName));

        internal static async Task AddRole(this GroupsModuleFacade facade, 
            Guid userId, 
            Guid groupId,
            Guid roleId,
            string roleName
            )
            => await facade.SendAsync(new AddRoleCommand(userId, groupId, roleId, roleName));

        internal static async Task AddMember(this GroupsModuleFacade facade,
            Guid groupId,
            Guid userIdOfMember
        )
            => await facade.SendAsync(new AddGroupMember(groupId, userIdOfMember));

        internal static async Task AssignRole(this GroupsModuleFacade facade,
            Guid userId,
            Guid groupId,
            Guid userIdOfMember,
            Guid roleId
        )
            => await facade.SendAsync(new AssignRoleToMemberCommand(userId, groupId, userIdOfMember, roleId));

        internal static async Task AddPermission(this GroupsModuleFacade facade,
            Guid userId,
            Guid groupId,
            Guid roleId,
            string permissionName
        )
            => await facade.SendAsync(new AddPermissionToRoleCommand(userId, groupId, roleId, permissionName));
    }
}