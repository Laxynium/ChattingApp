using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Groups.Infrastructure.Database.Migrations
{
    public partial class AddInvitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invitations",
                schema: "Groups",
                columns: table => new
                {
                    InvitationId = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    InvitationCode = table.Column<string>(maxLength: 8, nullable: false),
                    ExpirationPeriod = table.Column<TimeSpan>(nullable: true),
                    ExpirationStart = table.Column<DateTimeOffset>(nullable: true),
                    ExpirationTimeType = table.Column<string>(nullable: true),
                    UsageCounterType = table.Column<string>(nullable: true),
                    UsageCounter = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invitations", x => x.InvitationId);
                    table.ForeignKey(
                        name: "FK_Invitations_Groups",
                        column: x => x.GroupId,
                        principalSchema: "Groups",
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_GroupId",
                schema: "Groups",
                table: "Invitations",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Invitations_InvitationCode",
                schema: "Groups",
                table: "Invitations",
                column: "InvitationCode",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invitations",
                schema: "Groups");
        }
    }
}
