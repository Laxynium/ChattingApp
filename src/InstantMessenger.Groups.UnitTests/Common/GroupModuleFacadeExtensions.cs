using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstantMessenger.Groups.Application.Features.Channel.AddChannel;
using InstantMessenger.Groups.Application.Features.Channel.AllowPermissionForMember;
using InstantMessenger.Groups.Application.Features.Channel.AllowPermissionForRole;
using InstantMessenger.Groups.Application.Features.Channel.DenyPermissionForRole;
using InstantMessenger.Groups.Application.Features.Group.Create;
using InstantMessenger.Groups.Application.Features.Invitations.GenerateInvitationCode;
using InstantMessenger.Groups.Application.Features.Members.Add;
using InstantMessenger.Groups.Application.Features.Members.AssignRole;
using InstantMessenger.Groups.Application.Features.Members.RemoveRole;
using InstantMessenger.Groups.Application.Features.Roles.AddPermissionToRole;
using InstantMessenger.Groups.Application.Features.Roles.AddRole;
using InstantMessenger.Groups.Application.Queries;

namespace InstantMessenger.Groups.UnitTests.Common
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
        private void AddInvitation(InvitationDto invitation) => _group.AddInvitation(invitation);
        private void AddChannel(ChannelDto channel) => _group.AddChannel(channel);
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

            public AsUserBuilder CreateInvitation(ExpirationTimeCommandItem expirationTime, UsageCounterCommandItem usageCounter)
            {
                var invitation = new InvitationDto(Guid.NewGuid());
                _builder.AddInvitation(invitation);
                _builder.AddBuildAction(
                    async () =>
                    {
                        await _facade.SendAsync(
                            new GenerateInvitationCommand(_userIdContext, _group.GroupId, invitation.InvitationId, expirationTime, usageCounter)
                        );
                        var invitationDto = await _facade.QueryAsync(new GetInvitationQuery(_userIdContext, _group.GroupId,invitation.InvitationId));
                        invitation.Code = invitationDto.Code;
                    });

                return this;
            }

            public ChannelBuilder CreateChannel(string channelName)
            {
                var channel = new ChannelDto(Guid.NewGuid());
                _builder.AddChannel(channel);
                _builder.AddBuildAction(
                    async () => await _facade.SendAsync(new CreateChannelCommand(_userIdContext, _group.GroupId, channel.ChannelId, channelName))
                );
                return new ChannelBuilder(this,channel);
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

                public RoleBuilder AddPermissions(params string[] names)
                {
                    foreach (var name in names)
                    {
                        AddPermission(name);
                    }
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

            internal class ChannelBuilder
            {
                private readonly AsUserBuilder _builder;
                private readonly ChannelDto _channel;
                private readonly GroupDto _group;
                private readonly GroupsModuleFacade _groupsModuleFacade;

                public ChannelBuilder(AsUserBuilder builder, ChannelDto channel)
                {
                    _builder = builder;
                    _channel = channel;
                    _group = _builder._builder._group;
                    _groupsModuleFacade = _builder._facade;
                }

                public ChannelBuilder AllowPermissionForRole(int roleNumber, string permission)
                {
                    var role = _group.Role(roleNumber);
                    _builder.AddBuildAction(async () => await _groupsModuleFacade.SendAsync(new AllowPermissionForRoleCommand(_builder._userIdContext, _group.GroupId, _channel.ChannelId, role.RoleId, permission)));
                    return this;
                }
                public ChannelBuilder AllowPermissionForMember(int memberNumber, string permission)
                {
                    var member = _group.Member(memberNumber);
                    _builder.AddBuildAction(async () => await _groupsModuleFacade.SendAsync(new AllowPermissionForMemberCommand(_builder._userIdContext, _group.GroupId, _channel.ChannelId, member.UserId, permission)));
                    return this;
                }

                public ChannelBuilder DenyPermissionForRole(int roleNumber, string permission)
                {
                    var role = _group.Role(roleNumber);
                    _builder.AddBuildAction(async () => await _groupsModuleFacade.SendAsync(new DenyPermissionForRoleCommand(_builder._userIdContext, _group.GroupId, _channel.ChannelId, role.RoleId, permission)));
                    return this;
                }
                //public ChannelBuilder AllowPermissionForMember(string permission, Guid userIfOfMember)
                //{
                //    _builder.AddBuildAction(async () => await _groupsModuleFacade.AddPermission(_builder._userIdContext, _group.GroupId, _channel.RoleId, name));
                //    return this;
                //}

                public AsUserBuilder Build() => _builder;
            }

        }


        internal class GroupDto
        {
            private readonly List<RoleDto> _roles = new List<RoleDto>();
            private readonly List<MemberDto> _members = new List<MemberDto>();
            private readonly List<InvitationDto> _invitations = new List<InvitationDto>();
            private readonly List<ChannelDto> _channels = new List<ChannelDto>();
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
            internal InvitationDto Invitation(int i) => _invitations[i-1];
            internal ChannelDto Channel(int i) => _channels[i-1];
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

            internal GroupDto AddInvitation(InvitationDto invitation)
            {
                _invitations.Add(invitation);
                return this;
            }
            internal GroupDto AddChannel(ChannelDto invitation)
            {
                _channels.Add(invitation);
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

        internal class InvitationDto
        {
            internal Guid InvitationId { get; }
            internal string Code { get; set; }
            public InvitationDto(Guid invitationId)
            {
                InvitationId = invitationId;
            }
        }
        internal class ChannelDto
        {
            internal Guid ChannelId { get; }
            internal string Code { get; set; }
            public ChannelDto(Guid channelId)
            {
                ChannelId = channelId;
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
        internal static async Task RemoveRoleFromMember(this GroupsModuleFacade facade,
            Guid userId,
            Guid groupId,
            Guid userIdOfMember,
            Guid roleId
        )
            => await facade.SendAsync(new RemoveRoleFromMemberCommand(userId, groupId, userIdOfMember, roleId));

        internal static async Task AddPermission(this GroupsModuleFacade facade,
            Guid userId,
            Guid groupId,
            Guid roleId,
            string permissionName
        )
            => await facade.SendAsync(new AddPermissionToRoleCommand(userId, groupId, roleId, permissionName));
    }
}