using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Groups.Infrastructure.Database.Migrations
{
    public partial class InitGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Groups");

            migrationBuilder.CreateTable(
                name: "Groups",
                schema: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Groups",
                schema: "Groups");
        }
    }
}
