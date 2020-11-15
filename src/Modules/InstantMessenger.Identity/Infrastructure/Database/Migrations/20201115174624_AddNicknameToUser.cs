using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Identity.Infrastructure.Database.Migrations
{
    public partial class AddNicknameToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                schema: "Identity",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nickname",
                schema: "Identity",
                table: "Users");
        }
    }
}
