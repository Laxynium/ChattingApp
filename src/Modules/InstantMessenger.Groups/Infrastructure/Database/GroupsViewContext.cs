using System;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Infrastructure.Database
{
    public class GroupMessageDto
    {
        public Guid GroupId { get; private set; }
        public Guid ChannelId { get; private set; }
        public Guid SenderId { get; private set; }
        public string SenderName { get; private set; }
        public byte[] SenderAvatar { get; private set; }
        public string Content { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public GroupMessageDto(Guid groupId, Guid channelId, Guid senderId, string senderName, byte[] senderAvatar, string content, DateTimeOffset createdAt)
        {
            GroupId = groupId;
            ChannelId = channelId;
            SenderId = senderId;
            SenderName = senderName;
            SenderAvatar = senderAvatar;
            Content = content;
            CreatedAt = createdAt;
        }

    }
    public class GroupsViewContext : DbContext
    {
        public DbSet<GroupMessageDto> GroupMessages { get; set; }

        public GroupsViewContext(DbContextOptions<GroupsViewContext>options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GroupMessageDto>(
                gm =>
                {
                    gm.HasNoKey();
                    gm.ToView("View_GroupMessages","Groups");
                }
            );
        }
    }
}