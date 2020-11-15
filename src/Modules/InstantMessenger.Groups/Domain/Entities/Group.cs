using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
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

        public void AddRole(UserId userId, RoleName roleName)
        {
            if(!CanAddRole(userId))
                throw new InvalidOperationException("Given user cannot perform add role operation");

            if(roleName == RoleName.EveryOneRole)
                throw new ArgumentException("Role cannot be same named as @everyone role");

            var role = Role.Create(RoleId.Create(), roleName, GetPriority());
            if (_roles.Contains(role))
            {
                throw new InvalidOperationException("Role cannot be added twice");
            }

            _roles.Add(role);
        }

        public void AddMember(UserId userId, UserId newMemberUserId, MemberName memberName, IClock clock)
        {
            if(!CanAddMember(userId))
                throw new InvalidOperationException("Given user cannot perform add member operation");

            if(Exists(newMemberUserId))
                throw new InvalidComObjectException("There is already user with given userId in group");

            var everyoneRole = GetEverOneRole();
            var newMember = Member.Create(newMemberUserId, memberName, everyoneRole, clock);
            _members.Add(newMember);
        }

        public void AssignRole(UserId executingUserId, UserId userId, RoleId roleId)
        {
            var executingMember = GetMember(executingUserId);
            if(!CanAssignRole(executingUserId, userId))
                throw new InvalidOperationException("Given user cannot perform assign role operation");

            var member = GetMember(executingUserId);
            var role = GetRole(roleId);
            if(role is null) 
                throw new AggregateException("Role with given id doesn't exists");
            member.AddRole(role);
        }

        private Role GetRole(RoleId roleId) 
            => _roles.FirstOrDefault(x => x.Id == roleId);

        private bool Exists(UserId userId)
        {
            return _members.Any(x => x.UserId == userId);
        }

        private Member GetMember(UserId userId)
        {
            return _members.FirstOrDefault(x => x.UserId == userId) ?? throw new ArgumentException("There is no member with given userId");
        }

        private Role GetEverOneRole()
        {
            return _roles.First(x => x.Name == RoleName.EveryOneRole);
        }

        private bool CanAssignRole(UserId userId, UserId userToAssign)
        {
            var member = GetMember(userId);
            if (member.IsOwner)
                return true;
            var permissions = GetMemberPermissions(member);
            if(!permissions.Has(Permission.Administrator, Permission.ManageRoles))
                return false;

            var memberRoles = GetRoles(member);
            var memberToAssignRoles = GetRoles(GetMember(userToAssign));
            var highestRole = memberRoles.Where(x=>x.Permissions.Has(Permission.Administrator, Permission.ManageRoles)).OrderBy(x => x.Priority).First();
            return false;
        }

        private bool CanAddRole(UserId userId)
        {
            var member = GetMember(userId);
            if (member.IsOwner)
                return true;
            var memberPermissions = GetMemberPermissions(member);
            return memberPermissions.Has(Permission.Administrator, Permission.ManageRoles);
        }

        private bool CanAddMember(UserId userId)
        {
            var member = GetMember(userId);
            if (member.IsOwner)
                return true;
            var memberPermissions = GetMemberPermissions(member);
            return memberPermissions.Has(Permission.Administrator, Permission.ManageGroup);
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
            return maybeRole.Map(x => x.Priority.Decreased())
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