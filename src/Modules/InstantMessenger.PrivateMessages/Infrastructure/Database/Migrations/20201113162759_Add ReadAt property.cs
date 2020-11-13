using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.PrivateMessages.Infrastructure.Database.Migrations
{
    public partial class AddReadAtproperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ReadAt",
                schema: "PrivateMessages",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReadAt",
                schema: "PrivateMessages",
                table: "Messages");
        }
    }
}
