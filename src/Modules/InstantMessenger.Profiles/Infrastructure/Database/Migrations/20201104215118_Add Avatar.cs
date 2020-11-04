using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Profiles.Infrastructure.Database.Migrations
{
    public partial class AddAvatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                schema: "Profiles",
                table: "Profiles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                schema: "Profiles",
                table: "Profiles");
        }
    }
}
