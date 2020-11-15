using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;

namespace InstantMessenger.Groups.Infrastructure.Database
{
    internal sealed class GroupRepository : IGroupRepository
    {
        private readonly GroupsContext _context;

        public GroupRepository(GroupsContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Group @group)
        {
            await _context.AddAsync(group);
        }
    }
}