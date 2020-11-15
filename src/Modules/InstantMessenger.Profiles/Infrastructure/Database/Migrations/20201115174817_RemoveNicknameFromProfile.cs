using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Profiles.Infrastructure.Database.Migrations
{
    public partial class RemoveNicknameFromProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Nickname",
                schema: "Profiles",
                table: "Profiles");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                schema: "Profiles",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
