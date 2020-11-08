using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.PrivateMessages.Infrastructure.Database.Migrations
{
    public partial class InitPrivateMessages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "PrivateMessages");

            migrationBuilder.CreateTable(
                name: "Conversations",
                schema: "PrivateMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstParticipant = table.Column<Guid>(nullable: false),
                    SecondParticipant = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                schema: "PrivateMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    ConversationId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    From = table.Column<Guid>(nullable: false),
                    To = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Conversations",
                schema: "PrivateMessages");

            migrationBuilder.DropTable(
                name: "Messages",
                schema: "PrivateMessages");
        }
    }
}
