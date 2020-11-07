using System;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Friendships.Infrastructure.Database
{
    internal  sealed class PersonRepository : IPersonRepository
    {
        private readonly FriendshipsContext _context;

        public PersonRepository(FriendshipsContext context)
        {
            _context = context;
        }
        public async Task<bool> ExistsAsync(Guid personId) 
            => await _context.Persons.AnyAsync(x => x.Id == personId);

        public async Task AddAsync(Person person)
        {
            await _context.Persons.AddAsync(person);
        }

        public async Task<Person> GetAsync(Guid personId) 
            => await _context.Persons.FindAsync(personId);
    }
}