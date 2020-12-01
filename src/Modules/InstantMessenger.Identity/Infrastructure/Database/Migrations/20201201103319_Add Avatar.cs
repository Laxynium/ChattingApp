using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Identity.Infrastructure.Database.Migrations
{
    public partial class AddAvatar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Avatar",
                schema: "Identity",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Avatar",
                schema: "Identity",
                table: "Users");
        }
    }
}
