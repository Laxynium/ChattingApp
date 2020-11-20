using System;
using System.Collections.Generic;
using System.Linq;
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
            => new Member(userId, MemberId.Create(),name, true, everyoneRole.Id,clock.GetCurrentInstant().InUtc().ToDateTimeOffset());

        public static Member Create(UserId userId, MemberName name, Role everyoneRole, IClock clock) 
            => new Member(userId,MemberId.Create(),name, false, everyoneRole.Id, clock.GetCurrentInstant().InUtc().ToDateTimeOffset());

        public void AddRole(Role role)
        {
            if (_roles.Contains(role.Id))
                return;
            _roles.Add(role.Id);
        }

        public void RemoveRole(Role role)
        {
            if (!_roles.Contains(role.Id))
                return;
            _roles.Remove(role.Id);
        }
    }
}