using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.PrivateMessages.Infrastructure.Database.Migrations
{
    public partial class AddOutbox : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                schema: "PrivateMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OccurredOn = table.Column<DateTime>(nullable: false),
                    Data = table.Column<string>(nullable: false),
                    ProcessedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OutboxMessage",
                schema: "PrivateMessages");
        }
    }
}
