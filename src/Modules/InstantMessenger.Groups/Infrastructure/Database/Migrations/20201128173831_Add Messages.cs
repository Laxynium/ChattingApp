using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Groups.Infrastructure.Database.Migrations
{
    public partial class AddMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "Groups",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(nullable: false),
                    From = table.Column<Guid>(nullable: false),
                    ChannelId = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageId);
                    table.ForeignKey(
                        name: "FK_Messages_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalSchema: "Groups",
                        principalTable: "Channels",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChannelId",
                schema: "Groups",
                table: "Messages",
                column: "ChannelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Messages",
                schema: "Groups");
        }
    }
}
