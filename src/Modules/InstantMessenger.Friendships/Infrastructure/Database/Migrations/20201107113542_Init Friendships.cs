using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Friendships.Infrastructure.Database.Migrations
{
    public partial class InitFriendships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Friendships");

            migrationBuilder.CreateTable(
                name: "Persons",
                schema: "Friendships",
                columns: table => new
                {
                    PersonId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                schema: "Friendships",
                columns: table => new
                {
                    FriendshipId = table.Column<Guid>(nullable: false),
                    FirstPerson = table.Column<Guid>(nullable: false),
                    SecondPerson = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.FriendshipId);
                    table.ForeignKey(
                        name: "FK_FirstPerson_Friendships_Persons",
                        column: x => x.FirstPerson,
                        principalSchema: "Friendships",
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK_SecondPerson_Friendships_Persons",
                        column: x => x.SecondPerson,
                        principalSchema: "Friendships",
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                });

            migrationBuilder.CreateTable(
                name: "Invitations",
                schema: "Friendships",
                columns: table => new
                {
                    InvitationId = table.Column<Guid>(nullable: false),
                    SenderId = table.Column<Guid>(nullable: false),
                    ReceiverId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false),
                    Status = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.InvitationId);
                    table.ForeignKey(
                        name: "FK_Receiver_Invitations_Persons",
                        column: x => x.ReceiverId,
                        principalSchema: "Friendships",
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                    table.ForeignKey(
                        name: "FK_Sender_Invitations_Persons",
                        column: x => x.SenderId,
                        principalSchema: "Friendships",
                        principalTable: "Persons",
                        principalColumn: "PersonId");
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friendships",
                schema: "Friendships");

            migrationBuilder.DropTable(
                name: "Invitations",
                schema: "Friendships");

            migrationBuilder.DropTable(
                name: "Persons",
                schema: "Friendships");
        }
    }
}
