using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Profiles.Infrastructure.Database.Migrations
{
    public partial class InitProfiles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Profiles");

            migrationBuilder.CreateTable(
                name: "Profiles",
                schema: "Profiles",
                columns: table => new
                {
                    ProfileId = table.Column<Guid>(nullable: false),
                    Nickname = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileId", x => x.ProfileId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profiles",
                schema: "Profiles");
        }
    }
}
