using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Messages.Entities;
using InstantMessenger.Groups.Domain.Messages.ValueObjects;
using InstantMessenger.Groups.Domain.Rules;
using InstantMessenger.Groups.Domain.ValueObjects;
using NodaTime;
using Action = InstantMessenger.Groups.Domain.Entities.PermissionsVerification.Actions.Action;

namespace InstantMessenger.Groups.Domain.Entities
{
    public class Group : Entity<GroupId>
    {
        private readonly HashSet<Member> _members = new HashSet<Member>();
        public IEnumerable<Member> Members => _members.ToList();

        private readonly HashSet<Role> _roles = new HashSet<Role>();
        public IEnumerable<Role> Roles => _roles.ToList();
        public GroupName Name { get; }
        public DateTimeOffset CreatedAt { get; }

        private Group(){}

        private Group(GroupId id, GroupName name, DateTimeOffset createdAt):base(id)
        {
            Name = name;
            CreatedAt = createdAt;
        }

        public static Group Create(GroupId id, GroupName name, UserId userId, MemberName ownerName, IClock clock)
        {
            var group = new Group(id, name, clock.GetCurrentInstant().InUtc().ToDateTimeOffset());
            var everyoneRole = Role.CreateEveryone();
            group.WithRoles(everyoneRole);
            var owner = Member.CreateOwner(userId, ownerName, everyoneRole, clock);
            group.WithOwner(owner);
            return group;
        }

        public void Remove(UserId userId) => new Action.DeleteGroup(GetMember(userId))
            .Execute();

        public void LeaveGroup(UserId userId)
        {
            var member = GetMember(userId);
            if (member.IsOwner)
                throw new OwnerCannotLeaveGroupException();
            _members.Remove(member);
        }


        public void AddRole(UserId userId, RoleId roleId, RoleName roleName) => new Action.AddRole(GetMember(userId),GetMemberPermissions(userId))
        .Execute(() =>
        {
            if (roleName == RoleName.EveryOneRole)
                throw new InvalidRoleNameException(roleName);

            var role = Role.Create(roleId, roleName, GetPriority());
            if (_roles.Contains(role))
            {
                throw new RoleAlreadyExistsException(roleId);
            }

            foreach (var role1 in _roles)
            {
                role1.IncrementPriority();
            }
            _roles.Add(role);
        });

        public void RemoveRole(UserId userId, RoleId roleId) => new Action.RemoveRole(GetMember(userId), GetMemberPermissions(userId), GetRoles(userId), GetRole(roleId))
        .Execute(() =>
        {
            var role = GetRole(roleId);

            foreach (var m in _members)
            {
                m.RemoveRole(role);
            }

            _roles.Remove(role);
        });

        public void AddMember(UserId userIdOfMember, MemberName memberName, IClock clock)
        {
            if(Exists(userIdOfMember))
                throw new MemberAlreadyExistsException(userIdOfMember);

            var everyoneRole = GetEveryoneRole();
            var newMember = Member.Create(userIdOfMember, memberName, everyoneRole, clock);
            _members.Add(newMember);
        }

        public void KickMember(UserId userId, UserId userIdOfMember) => new Action.KickMember(GetMember(userId), GetMemberPermissions(userId),GetRoles(userId),GetMember(userIdOfMember), GetRoles(userIdOfMember))
            .Execute(() =>
            {
                _members.Remove(GetMember(userIdOfMember));

            });

        public bool ContainsMember(UserId userId)
        {
            return _members.Any(x => x.UserId == userId);
        }

        public void AssignRole(UserId userId, UserId userIdOfMember, RoleId roleId) => new Action.AssignRole(GetMember(userId),GetMemberPermissions(userId),GetRoles(userId),UserDefinedRoles, GetRole(roleId))
        .Execute(() =>
        {
            var member = GetMember(userIdOfMember);
            member.AddRole(GetRole(roleId));
        });

        public void RemoveRoleFromMember(UserId userId, UserId userIdOfMember, RoleId roleId) => new Action.RemoveRoleFromMember(GetMember(userId), GetMemberPermissions(userId), GetRoles(userId), UserDefinedRoles, GetRole(roleId))
            .Execute(() =>
        {
            var member = GetMember(userIdOfMember);
            member.RemoveRole(GetRole(roleId));
        });


        public void AddPermissionToRole(UserId userId, RoleId roleId, Permission permission) => new Action.AddPermissionToRole(GetMember(userId), GetMemberPermissions(userId),GetRoles(userId),GetRole(roleId),permission)
        .Execute(() =>
        {
            GetRole(roleId).AddPermission(permission);
        });

        public void RemovePermissionFromRole(UserId userId, RoleId roleId, Permission permission) => new Action.RemovePermissionFromRole(GetMember(userId), GetMemberPermissions(userId), GetRoles(userId), GetRole(roleId), permission)
        .Execute(() =>
        {
            GetRole(roleId).RemovePermission(permission);
        });

        public void MoveUpRole(UserId userId, RoleId roleId) => new Action.MoveUpRoleInHierarchy(GetMember(userId),GetMemberPermissions(userId),GetRoles(userId),UserDefinedRoles, GetRole(roleId))
        .Execute(() =>
        {
            var roleAbove = GetRoleAbove(GetRole(roleId));
            if (roleAbove.HasNoValue)//given role is the highest one
                return;
            GetRole(roleId).IncrementPriority();
            roleAbove.Value.DecrementPriority();
        });

        public void MoveDownRole(UserId userId, RoleId roleId) => new Action.MoveUpRoleInHierarchy(GetMember(userId), GetMemberPermissions(userId), GetRoles(userId), UserDefinedRoles, GetRole(roleId))
        .Execute(() =>
        {
            var roleBelow = GetRoleBelow(GetRole(roleId));
            if (roleBelow.HasNoValue)//given role is the lowest one
                return;
            GetRole(roleId).DecrementPriority();
            roleBelow.Value.IncrementPriority();
        });

        public async Task<Invitation> GenerateInvitation(UserId userId, InvitationId invitationId, ExpirationTime expirationTime,
            UsageCounter usageCounter, IUniqueInvitationCodeRule uniqueInvitationCodeRule)
        {
            new Action.GenerateInvitation(GetMember(userId),GetMemberPermissions(userId)).Execute();
            
            return await Invitation.Create(invitationId, Id, expirationTime, usageCounter, uniqueInvitationCodeRule);
        }

        public void RevokeInvitation(UserId userId, Invitation invitation) => 
            new Action.RevokeInvitation(GetMember(userId), GetMemberPermissions(userId))
        .Execute(
        () =>
        {
            invitation.Revoke(userId);
        });

        public Channel CreateChannel(UserId userId, ChannelId channelId, ChannelName channelName)
        {
            new Action.AddChannel(GetMember(userId), GetMemberPermissions(userId))
                .Execute();
            var channel = new Channel(channelId,this.Id, channelName);
            return channel;
        }


        public void RemoveChannel(UserId userId, Channel channel) => new Action.RemoveChannel(GetMember(userId),GetMemberPermissions(userId),channel,GetEveryoneRole())
            .Execute();

        public void AllowPermission(UserId userId, Channel channel, RoleId roleId, Permission permission) => new Action.AllowPermissionForRole(GetMember(userId), GetMemberPermissions(userId), channel, GetEveryoneRole(), permission)
            .Execute(() =>
        {
            var role = GetRole(roleId);

            channel.AllowPermission(role, permission);
        });

        public void DenyPermission(UserId userId, Channel channel, RoleId roleId, Permission permission) => new Action.AllowPermissionForRole(GetMember(userId), GetMemberPermissions(userId), channel, GetEveryoneRole(), permission)
            .Execute(() =>
        {
            channel.DenyPermission(GetRole(roleId), permission);
        });

        public void RemoveOverride(UserId userId, Channel channel, Permission permission, RoleId roleId) => new Action.AllowPermissionForRole(GetMember(userId), GetMemberPermissions(userId), channel, GetEveryoneRole(), permission)
            .Execute(() =>
        {
            channel.RemoveOverride(GetRole(roleId), permission);
        });

        public void AllowPermission(UserId userId, Channel channel, UserId userIdOfMember, Permission permission) 
            => new Action.AllowPermissionForMember(GetMember(userId), GetMemberPermissions(userId), channel, GetEveryoneRole(), permission)
            .Execute(() =>
        {
            channel.AllowPermission(GetMember(userIdOfMember), permission);
        });

        public void DenyPermission(UserId userId, Channel channel, UserId userIdOfMember, Permission permission) 
            => new Action.DenyPermissionForMember(GetMember(userId), GetMemberPermissions(userId), channel, GetEveryoneRole(), permission)
            .Execute(() =>
        {
            channel.DenyPermission(GetMember(userIdOfMember), permission);
        });

        public void RemoveOverride(UserId userId, Channel channel, Permission permission, UserId userIfOfMember) 
            => new Action.RemoveOverrideForMember(
                    GetMember(userId), 
                    GetMemberPermissions(userId), 
                    channel, 
                    GetEveryoneRole(), 
                    permission).Execute(() =>
        {
            channel.RemoveOverride(GetMember(userIfOfMember), permission);
        });

        public Message SendMessage(UserId userId, Channel channel, MessageId messageId, MessageContent content, IClock clock)
        {
            new Action.SendMessage(GetMember(userId), GetMemberPermissions(userId), channel, GetEveryoneRole())
                .Execute(()=>{});

            var message = new Message(messageId, userId, channel.Id, content,clock.GetCurrentInstant().InUtc().ToDateTimeOffset());

            return message;
        }

        public IEnumerable<Channel> GetChannels(UserId userId)
        {
            //select only those which should be visible to user
            yield break;
        }

        public bool CanAccessInvitations(UserId userId)
        {
            return new Action.RevokeInvitation(GetMember(userId), GetMemberPermissions(userId))
                .CanExecute();
        }

        private Maybe<Role> GetRoleAbove(Role role) => 
            UserDefinedRoles.Skip(1)
            .Zip(UserDefinedRoles)
            .Where(x => x.First == role)
            .Select(x => x.Second)
            .TryFirst();

        private Maybe<Role> GetRoleBelow(Role role) => 
            UserDefinedRoles
            .Zip(UserDefinedRoles.Skip(1))
            .Where(x => x.First == role)
            .Select(x => x.Second)
            .TryFirst();

        private List<Role> UserDefinedRoles => _roles
            .Where(x => x.Name != RoleName.EveryOneRole)
            .OrderByDescending(x => x.Priority)
            .ToList();

        private Role GetRole(RoleId roleId)
            => _roles.FirstOrDefault(x => x.Id == roleId) ?? throw new RoleNotFoundException(roleId);

        private bool Exists(UserId userId) 
            => _members.Any(x => x.UserId == userId);

        private Member GetMember(UserId userId) 
            => _members.FirstOrDefault(x => x.UserId == userId) ?? throw new MemberNotFoundException(userId);

        private Role GetEveryoneRole() 
            => _roles.First(x => x.Name == RoleName.EveryOneRole);


        private Permissions GetMemberPermissions(UserId userId)
        {
            var memberRoles = GetRoles(userId);
            var memberPermissions = memberRoles.Select(x => x.Permissions).Aggregate(Permissions.Empty(), (agg, p) => agg.Add(p));
            return memberPermissions;
        }

        private IEnumerable<Role> GetRoles(UserId userId)
        {
            var member = GetMember(userId);
            return _roles.Where(x => member.Roles.Contains(x.Id));
        }

        private RolePriority GetPriority()
        {
            var maybeRole = _roles.OrderBy(r => r.Priority).TakeWhile(x=>x.Priority != RolePriority.Lowest).TryFirst();
            return maybeRole.Map(x => x.Priority.Increased())
                .Unwrap(RolePriority.CreateFirst());
        }

        private void WithRoles(params Role[] roles)
        {
            foreach (var role in roles)
            {
                _roles.Add(role);
            }
        }

        private void WithOwner(Member owner)
        {
            _members.Add(owner);
        }
    }
}