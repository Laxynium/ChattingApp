using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Groups.Infrastructure.Database.Migrations
{
    public partial class AddMessagesView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
CREATE OR ALTER VIEW Groups.vw_GroupMessages AS
SELECT m.GroupId
	, m.ChannelId
    , m.MessageId
	, m.[From] AS SenderId
	, mem.Name AS SenderName
	, mem.Avatar AS SenderAvatar
	, m.Content
	, m.CreatedAt
FROM Groups.Messages m
JOIN Groups.Members mem ON mem.GroupId = m.GroupId AND mem.UserId = m.[From]"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP VIEW IF EXISTS Groups.vw_GroupMessages"
            );
        }
    }
}
