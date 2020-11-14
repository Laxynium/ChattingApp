using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Infrastructure;

namespace InstantMessenger.Groups.Api.Features.GroupCreation
{
    public interface IGroupRepository
    {
        Task AddAsync(Group group);
        Task SaveAsync();
    }

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

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}