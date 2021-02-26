using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Groups.Infrastructure.Database.ViewMigrations
{
    public partial class RemoveMessagesView : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
DROP VIEW IF EXISTS Groups.View_GroupMessages"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
