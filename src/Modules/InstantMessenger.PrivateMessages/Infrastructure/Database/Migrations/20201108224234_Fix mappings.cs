using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.PrivateMessages.Infrastructure.Database.Migrations
{
    public partial class Fixmappings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "SecondParticipant",
                schema: "PrivateMessages",
                table: "Conversations",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "FirstParticipant",
                schema: "PrivateMessages",
                table: "Conversations",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "SecondParticipant",
                schema: "PrivateMessages",
                table: "Conversations",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FirstParticipant",
                schema: "PrivateMessages",
                table: "Conversations",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);
        }
    }
}
