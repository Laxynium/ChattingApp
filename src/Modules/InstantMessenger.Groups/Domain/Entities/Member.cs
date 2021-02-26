using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.SharedKernel;
using NodaTime;

namespace InstantMessenger.Groups.Domain.Entities
{
    public class Member : Entity<MemberId>
    {
        private readonly HashSet<RoleId> _roles = new();
        public IEnumerable<RoleId> Roles => _roles.ToList();
        public UserId UserId { get; }//required in order to map user to member in particular group
        public MemberName Name { get; }
        public Avatar? Avatar { get; }
        public bool IsOwner { get; }
        public DateTimeOffset CreatedAt { get; }

        private Member(){}
        private Member(UserId userId, MemberId id, MemberName name, Avatar? avatar, bool isOwner, RoleId role, DateTimeOffset createdAt): base(id)
        {
            UserId = userId;
            Name = name;
            Avatar = avatar;
            IsOwner = isOwner;
            _roles.Add(role);
            CreatedAt = createdAt;
        }

        public static Member CreateOwner(UserId userId, MemberName name, Avatar? avatar, Role everyoneRole, IClock clock) 
            => new(userId, MemberId.Create(),name, avatar,true, everyoneRole.Id,clock.GetCurrentInstant().InUtc().ToDateTimeOffset());

        public static Member Create(UserId userId, MemberName name, Avatar? avatar, Role everyoneRole, IClock clock) 
            => new(userId,MemberId.Create(),name, avatar,false, everyoneRole.Id, clock.GetCurrentInstant().InUtc().ToDateTimeOffset());

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