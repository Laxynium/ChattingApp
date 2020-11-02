using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Identity.Infrastructure.Database.Migrations
{
    public partial class RenameVerificationLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerificationLinks",
                schema: "Identity");

            migrationBuilder.CreateTable(
                name: "ActivationLinks",
                schema: "Identity",
                columns: table => new
                {
                    ActivationLinkId = table.Column<Guid>(nullable: false),
                    Token = table.Column<string>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivationLinks", x => x.ActivationLinkId);
                    table.ForeignKey(
                        name: "FK_ActivationLinks_Users",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActivationLinks_UserId",
                schema: "Identity",
                table: "ActivationLinks",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivationLinks",
                schema: "Identity");

            migrationBuilder.CreateTable(
                name: "VerificationLinks",
                schema: "Identity",
                columns: table => new
                {
                    VerificationLinkId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationLinks", x => x.VerificationLinkId);
                    table.ForeignKey(
                        name: "FK_VerificationLinks_Users",
                        column: x => x.UserId,
                        principalSchema: "Identity",
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VerificationLinks_UserId",
                schema: "Identity",
                table: "VerificationLinks",
                column: "UserId");
        }
    }
}
