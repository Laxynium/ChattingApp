﻿using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Infrastructure.Database.Repositories
{
    internal sealed class ChannelRepository : IChannelRepository
    {
        private readonly GroupsContext _context;

        public ChannelRepository(GroupsContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Channel channel)
        {
            await _context.Channels.AddAsync(channel);
        }

        public async Task<Channel> GetAsync(GroupId groupId, ChannelId channelId) 
            => await _context.Channels
                .FirstOrDefaultAsync(x=>x.Id == channelId && x.GroupId == groupId);

        public Task RemoveAsync(Channel channel)
        {
            _context.Remove(channel);
            return Task.CompletedTask;
        }

        public async Task<bool> ExistsAsync(GroupId groupId, ChannelId channelId)
        {
            return await _context.Channels.AnyAsync(c => c.GroupId == groupId && c.Id == channelId);
        }
    }
}