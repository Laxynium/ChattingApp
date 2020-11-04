﻿using System.Threading.Tasks;

namespace InstantMessenger.Profiles
{
    internal sealed class UnitOfWork : IUnitOfWork
    {
        private readonly ProfilesContext _context;

        public UnitOfWork(ProfilesContext context)
        {
            _context = context;
        }
        public async Task Commit()
        {
            await _context.SaveChangesAsync();
        }
    }
}