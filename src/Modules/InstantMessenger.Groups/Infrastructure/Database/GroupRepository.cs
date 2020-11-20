using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Infrastructure.Database
{
    internal sealed class GroupRepository : IGroupRepository
    {
        private readonly GroupsContext _context;

        public GroupRepository(GroupsContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Group group)
        {
            await _context.AddAsync(group);
        }

        public async Task<Group> GetAsync(GroupId id) 
            => await _context.Groups.FirstOrDefaultAsync(x=>x.Id == id);

        public async Task<bool> ExistsAsync(GroupId id) 
            => await _context.Groups.AnyAsync(x => x.Id == id);
    }
}