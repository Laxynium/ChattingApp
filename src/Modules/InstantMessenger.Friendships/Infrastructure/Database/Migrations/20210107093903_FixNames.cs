using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Friendships.Infrastructure.Database.Migrations
{
    public partial class FixNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("FirstPerson", "Friendships", "FirstPersonId", "Friendships");
            migrationBuilder.RenameColumn("SecondPerson", "Friendships", "SecondPersonId", "Friendships");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn("FirstPersonId", "Friendships", "FirstPerson", "Friendships");
            migrationBuilder.RenameColumn("SecondPersonId", "Friendships", "SecondPerson", "Friendships");
        }
    }
}
