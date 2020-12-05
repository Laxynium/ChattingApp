using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Friendships.Infrastructure.Database.Migrations
{
    public partial class ChangePersonToValueObject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FirstPerson_Friendships_Persons",
                schema: "Friendships",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_SecondPerson_Friendships_Persons",
                schema: "Friendships",
                table: "Friendships");

            migrationBuilder.DropForeignKey(
                name: "FK_Receiver_Invitations_Persons",
                schema: "Friendships",
                table: "Invitations");

            migrationBuilder.DropForeignKey(
                name: "FK_Sender_Invitations_Persons",
                schema: "Friendships",
                table: "Invitations");

            migrationBuilder.DropTable(
                name: "Persons",
                schema: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_ReceiverId",
                schema: "Friendships",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Invitations_SenderId",
                schema: "Friendships",
                table: "Invitations");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_FirstPerson",
                schema: "Friendships",
                table: "Friendships");

            migrationBuilder.DropIndex(
                name: "IX_Friendships_SecondPerson",
                schema: "Friendships",
                table: "Friendships");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                schema: "Friendships",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_ReceiverId",
                schema: "Friendships",
                table: "Invitations",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_SenderId",
                schema: "Friendships",
                table: "Invitations",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_FirstPerson",
                schema: "Friendships",
                table: "Friendships",
                column: "FirstPerson");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_SecondPerson",
                schema: "Friendships",
                table: "Friendships",
                column: "SecondPerson");

            migrationBuilder.AddForeignKey(
                name: "FK_FirstPerson_Friendships_Persons",
                schema: "Friendships",
                table: "Friendships",
                column: "FirstPerson",
                principalSchema: "Friendships",
                principalTable: "Persons",
                principalColumn: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_SecondPerson_Friendships_Persons",
                schema: "Friendships",
                table: "Friendships",
                column: "SecondPerson",
                principalSchema: "Friendships",
                principalTable: "Persons",
                principalColumn: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Receiver_Invitations_Persons",
                schema: "Friendships",
                table: "Invitations",
                column: "ReceiverId",
                principalSchema: "Friendships",
                principalTable: "Persons",
                principalColumn: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sender_Invitations_Persons",
                schema: "Friendships",
                table: "Invitations",
                column: "SenderId",
                principalSchema: "Friendships",
                principalTable: "Persons",
                principalColumn: "PersonId");
        }
    }
}
