using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Groups.Infrastructure.Database.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "FK_Members",
                schema: "Groups",
                table: "Members");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                schema: "Groups",
                table: "Members",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "MemberRoles",
                schema: "Groups",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(nullable: false),
                    MemberId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRoles", x => new { x.RoleId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_MemberRoles_Members_MemberId",
                        column: x => x.MemberId,
                        principalSchema: "Groups",
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    Permissions = table.Column<int>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Groups",
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberRoles_MemberId",
                schema: "Groups",
                table: "MemberRoles",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_GroupId",
                schema: "Groups",
                table: "Roles",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberRoles",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "Groups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                schema: "Groups",
                table: "Members");

            migrationBuilder.AddPrimaryKey(
                name: "FK_Members",
                schema: "Groups",
                table: "Members",
                column: "Id");
        }
    }
}
