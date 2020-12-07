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

        public void Remove(UserId userId)
        {
            var member = GetMember(userId);
            if(!member.IsOwner)
                throw new InsufficientPermissionsException(userId);
        }


        public void AddRole(UserId userId, RoleId roleId, RoleName roleName)
        {
            if (!CanAddRole(userId))
                throw new InsufficientPermissionsException(userId);

            if(roleName == RoleName.EveryOneRole)
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
        }

        public void RemoveRole(UserId userId, RoleId roleId)
        {
            if (!CanRemoveRole(userId, roleId))
            {
                throw new InsufficientPermissionsException(userId);
            }

            var role = GetRole(roleId);

            foreach (var member in _members)
            {
                member.RemoveRole(role);
            }
            _roles.Remove(role);
        }

        public void AddMember(UserId userIdOfMember, MemberName memberName, IClock clock)
        {
            if(Exists(userIdOfMember))
                throw new MemberAlreadyExistsException(userIdOfMember);

            var everyoneRole = GetEverOneRole();
            var newMember = Member.Create(userIdOfMember, memberName, everyoneRole, clock);
            _members.Add(newMember);
        }

        public void AssignRole(UserId userId, UserId userIdOfMember, RoleId roleId)
        {
            if(!CanAssignOrRemoveRoleFromMember(userId, roleId))
                throw new InsufficientPermissionsException(userId);

            var member = GetMember(userIdOfMember);
            var role = GetRole(roleId);
            member.AddRole(role);
        }

        public void RemoveRoleFromMember(UserId userId, UserId userIdOfMember, RoleId roleId)
        {
            if (!CanAssignOrRemoveRoleFromMember(userId, roleId))
                throw new InsufficientPermissionsException(userId);

            var member = GetMember(userIdOfMember);
            var role = GetRole(roleId);
            member.RemoveRole(role);
        }

        public void AddPermissionToRole(UserId userId, RoleId roleId, Permission permission)
        {
            if(!CanManagePermissions(userId, roleId, permission))
                throw new InsufficientPermissionsException(userId);

            var role = GetRole(roleId);
            role.AddPermission(permission);
        }

        public void RemovePermissionFromRole(UserId userId, RoleId roleId, Permission permission)
        {
            if(!CanManagePermissions(userId, roleId, permission))
                throw new InsufficientPermissionsException(userId);

            var role = GetRole(roleId);
            role.RemovePermission(permission);
        }

        public void MoveUpRole(UserId userId, RoleId roleId) => MoveRole(userId,roleId,r =>
        {
            var roleAbove = GetRoleAbove(r);
            if (roleAbove.HasNoValue)//given role is the highest one
                return;
            r.IncrementPriority();
            roleAbove.Value.DecrementPriority();
        });

        public void MoveDownRole(UserId userId, RoleId roleId) => MoveRole(userId, roleId, r =>
        {
            var roleBelow = GetRoleBelow(r);
            if (roleBelow.HasNoValue)//given role is the lowest one
                return;
            r.DecrementPriority();
            roleBelow.Value.IncrementPriority();
        });

        public async Task<Invitation> GenerateInvitation(UserId userId, InvitationId invitationId, ExpirationTime expirationTime,
            UsageCounter usageCounter, IUniqueInvitationCodeRule uniqueInvitationCodeRule)
        {
            var member = GetMember(userId);
            if(!CanGenerateInvitation(member))
                throw new InsufficientPermissionsException(member.UserId);

            return await Invitation.Create(invitationId, Id, expirationTime, usageCounter, uniqueInvitationCodeRule);
        }

        public bool ContainsMember(UserId userId)
        {
            return _members.Any(x => x.UserId == userId);
        }

        public void LeaveGroup(UserId userId)
        {
            var member = GetMember(userId);
            if (member.IsOwner)
                throw new OwnerCannotLeaveGroupException();
            _members.Remove(member);
        }

        public void KickMember(UserId userId, UserId userIdOfMember)
        {
            var asMember = GetMember(userId);
            var onMember = GetMember(userIdOfMember);
            if (!CanKickMember(asMember, onMember))
                throw new InsufficientPermissionsException(userId);

            _members.Remove(onMember);
        }

        public Channel CreateChannel(UserId userId, ChannelId channelId, ChannelName channelName)
        {
            if(!CanCreateChannel(userId))
                throw new InsufficientPermissionsException(userId);

            var channel = new Channel(channelId,this.Id, channelName);
            return channel;
        }

        public void AllowPermission(UserId userId, Channel channel, RoleId roleId, Permission permission)
        {
            var asMember = GetMember(userId);
            if(!CanApplyOverrideOnChannel(asMember, channel, permission))
                throw new InsufficientPermissionsException(userId);

            var role = GetRole(roleId);

            channel.AllowPermission(role, permission);
        }
        public void DenyPermission(UserId userId, Channel channel, RoleId roleId, Permission permission)
        {
            var asMember = GetMember(userId);
            if(!CanApplyOverrideOnChannel(asMember,channel, permission))
                throw new InsufficientPermissionsException(userId);

            channel.DenyPermission(GetRole(roleId), permission);
        }

        public void DenyPermission(UserId userId, Channel channel, UserId userIdOfMember, Permission permission)
        {
            var asMember = GetMember(userId);
            if (!CanApplyOverrideOnChannel(asMember, channel, permission))
                throw new InsufficientPermissionsException(userId);

            channel.DenyPermission(GetMember(userIdOfMember), permission);
        }

        public void AllowPermission(UserId userId, Channel channel, UserId userIdOfMember, Permission permission)
        {
            var asMember = GetMember(userId);
            if (!CanApplyOverrideOnChannel(asMember, channel, permission))
                throw new InsufficientPermissionsException(userId);

            channel.AllowPermission(GetMember(userIdOfMember), permission);
        }

        public void RemoveChannel(UserId userId, Channel channel)
        {
            var member = GetMember(userId);
            if (member.IsOwner)
                return;
            var permissions = GetMemberPermissions(member);
            if (permissions.Has(Permission.Administrator))
                return;

            var everyoneRole = GetEverOneRole();
            permissions = channel.CalculatePermissions(permissions, member, everyoneRole.Id);


            if (permissions.Has(Permission.ManageChannels))
                return;

            throw new InsufficientPermissionsException(userId);
        }

        public void RemoveOverride(UserId userId, Channel channel, Permission permission, UserId userIfOfMember)
        {
            var asMember = GetMember(userId);
            var onMember = GetMember(userIfOfMember);

            if (!CanApplyOverrideOnChannel(asMember, channel,permission))
                throw new InsufficientPermissionsException(userId);

            channel.RemoveOverride(onMember, permission);
        }
        public void RemoveOverride(UserId userId, Channel channel, Permission permission, RoleId roleId)
        {
            var asMember = GetMember(userId);
            var role = GetRole(roleId);

            if (!CanApplyOverrideOnChannel(asMember, channel, permission))
                throw new InsufficientPermissionsException(userId);

            channel.RemoveOverride(role, permission);
        }

        private bool CanApplyOverrideOnChannel(Member asMember, Channel channel, Permission permission)
        {
            if (asMember.IsOwner)
                return true;

            var memberPermissions = GetMemberPermissions(asMember);
            if (memberPermissions.Has(Permission.Administrator))
                return true;

            if (channel.CalculatePermissions(asMember, GetEverOneRole().Id).Has(Permission.ManageRoles))
                return true;

            var calculatedPermissions = channel.CalculatePermissions(memberPermissions, asMember, GetEverOneRole().Id);

            return calculatedPermissions.Has(permission) && permission != Permission.ManageRoles;
        }

        private bool CanGenerateInvitation(Member member)
        {
            if (member.IsOwner)
                return true;
            var permissions = GetMemberPermissions(member);
            return permissions.Has(Permission.Administrator, Permission.Invite);
        }

        private void MoveRole(UserId userId, RoleId roleId, Action<Role> action)
        {
            if (!CanMoveRole(userId, roleId))
                throw new InsufficientPermissionsException(userId);

            var role = GetRole(roleId);
            action(role);
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

        private Role GetEverOneRole() 
            => _roles.First(x => x.Name == RoleName.EveryOneRole);

        private bool CanAssignOrRemoveRoleFromMember(UserId asUserId, RoleId roleId)
        {
            var asMember = GetMember(asUserId);
            if (asMember.IsOwner)
                return true;
            var permissions = GetMemberPermissions(asMember);
            if(!permissions.Has(Permission.Administrator, Permission.ManageRoles))
                return false;

            var highestRole = GetRoleWithHighestPriority(asMember);
            var role = GetRole(roleId);
            return highestRole.Priority > role.Priority;

        }

        private bool CanAddRole(UserId userId)
        {
            var member = GetMember(userId);
            if (member.IsOwner)
                return true;
            var memberPermissions = GetMemberPermissions(member);
            return memberPermissions.Has(Permission.Administrator, Permission.ManageRoles);
        }

        private bool CanRemoveRole(UserId userId, RoleId roleId)
        {
            var member = GetMember(userId);
            if (member.IsOwner)
                return true;
            var memberPermissions = GetMemberPermissions(member);
            if(!memberPermissions.Has(Permission.Administrator, Permission.ManageRoles))
                return false;

            var memberHighestRole = GetRoleWithHighestPriority(member);
            var role = GetRole(roleId);
            if (role.Priority >= memberHighestRole.Priority)
                return false;

            return true;
        }

        private bool CanMoveRole(UserId userId, RoleId roleId)
        {
            var member = GetMember(userId);
            if (member.IsOwner)
                return true;

            var permissions = GetMemberPermissions(member);
            if (!permissions.Has(Permission.Administrator, Permission.ManageRoles))
                return false;

            var memberHighestRole = GetRoleWithHighestPriority(member);
            var role = GetRole(roleId);
            if (role.Priority >= memberHighestRole.Priority)
                return false;

            return true;
        }

        private bool CanManagePermissions(UserId userId, RoleId roleId, Permission permission)
        {
            var member = GetMember(userId);
            if (member.IsOwner)
                return true;

            var permissions = GetMemberPermissions(member);
            if (!permissions.Has(Permission.Administrator, Permission.ManageRoles))
                return false;

            var memberHighestRole = GetRoleWithHighestPriority(member);
            var role = GetRole(roleId);
            if (role.Priority >= memberHighestRole.Priority) 
                return false;

            if(!permissions.Has(Permission.Administrator) && !permissions.Has(permission))
                return false;

            return true;
        }

        private Role GetRoleWithHighestPriority(Member member)
        {
            var asMemberRoles = GetRoles(member);
            var highestRole = asMemberRoles.OrderByDescending(x => x.Priority).First();
            return highestRole;
        }

        private Permissions GetMemberPermissions(Member member)
        {
            var memberRoles = GetRoles(member);
            var memberPermissions = memberRoles.Select(x => x.Permissions).Aggregate(Permissions.Empty(), (agg, p) => agg.Add(p));
            return memberPermissions;
        }

        private IEnumerable<Role> GetRoles(Member member)
        {
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

        private bool CanKickMember(Member asMember, Member onMember)
        {
            if (onMember.IsOwner)
                return false;

            if (asMember == onMember)
                return false;

            if (asMember.IsOwner)
                return true;
            
            var permissions = GetMemberPermissions(asMember);
            if (!permissions.Has(Permission.Administrator, Permission.Kick))
                return false;

            var asMemberHighestRole = GetRoleWithHighestPriority(asMember);
            var onMemberHighestRole = GetRoleWithHighestPriority(onMember);

            if (asMemberHighestRole.Priority <= onMemberHighestRole.Priority)
                return false;

            return true;
        }

        private bool CanCreateChannel(UserId userId)
        {
            var member = GetMember(userId);
            if (member.IsOwner)
                return true;

            var permissions = GetMemberPermissions(member);
            if (!permissions.Has(Permission.Administrator, Permission.ManageChannels))
                return false;

            return true;
        }

        public Message SendMessage(UserId userId, Channel channel, MessageId messageId, MessageContent content, IClock clock)
        {
            var member = GetMember(userId);
            if(!CanSendMessage(member, channel))
                throw new InsufficientPermissionsException(userId);

            var message = new Message(messageId, userId, channel.Id, content,clock.GetCurrentInstant().InUtc().ToDateTimeOffset());

            return message;
        }

        private bool CanSendMessage(Member member, Channel channel)
        {
            if (member.IsOwner)
                return true;
            var permissions = GetMemberPermissions(member);
            if (permissions.Has(Permission.Administrator))
                return true;

            permissions = channel.CalculatePermissions(permissions, member, GetEverOneRole().Id);

            if (permissions.Has(Permission.SendMessages))
                return true;

            return false;
        }

    }
}