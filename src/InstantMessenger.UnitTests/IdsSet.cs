using System;
using System.Collections.Generic;

namespace InstantMessenger.UnitTests
{
    internal class IdsSet
    {
        private readonly Dictionary<int,Guid>_userIdOfMembers = new Dictionary<int, Guid>();
        private readonly Dictionary<int,Guid>_roleIds = new Dictionary<int, Guid>();
        public Guid UserId { get; } = new Guid();
        public Guid GroupId { get; } = new Guid();

        public Guid RoleId(int i)
        {
            if (!_roleIds.ContainsKey(i))
                _roleIds.Add(i, Guid.NewGuid());
            return _roleIds[i];
        }

        public Guid UserIdOfMember(int i)
        {
            if(!_userIdOfMembers.ContainsKey(i))
                _userIdOfMembers.Add(i, Guid.NewGuid());
            return _userIdOfMembers[i];
        }
    }
}