using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.GroupCreation;
using InstantMessenger.Groups.Domain;

namespace InstantMessenger.UnitTests
{
    internal sealed class FakeRepository : IGroupRepository
    {
        private readonly ISet<Group> _groups = new HashSet<Group>();
        private readonly List<Func<Group>> _groupsToSave = new List<Func<Group>>();
        public FakeRepository()
        {
            
        }

        public Task AddAsync(Group @group)
        {
            _groupsToSave.Add(() => @group);
            return Task.CompletedTask;
        }

        public Task SaveAsync()
        {
            _groupsToSave.ForEach(x=>_groups.Add(x()));
            _groupsToSave.Clear();
            return Task.CompletedTask;
        }
    }
}