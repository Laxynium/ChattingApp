using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Groups.Infrastructure.Database.ViewMigrations
{
    public partial class UpdateGroupMessagesView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE OR ALTER VIEW Groups.View_GroupMessages AS
SELECT c.GroupId
	, c.ChannelId
    , m.MessageId
	, m.[From] AS SenderId
	, mem.Name AS SenderName
	, u.Avatar AS SenderAvatar
	, m.Content
	, m.CreatedAt
FROM Groups.Messages m
JOIN Groups.Channels c ON c.ChannelId = m.ChannelId
JOIN Groups.Members mem ON mem.GroupId = c.GroupId AND mem.UserId = m.[From]
JOIN [Identity].Users u ON u.UserId = m.[From]"
            );

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
