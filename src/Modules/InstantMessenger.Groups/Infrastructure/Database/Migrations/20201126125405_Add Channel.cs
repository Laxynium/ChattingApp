using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Groups.Infrastructure.Database.Migrations
{
    public partial class AddChannel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Channels",
                schema: "Groups",
                columns: table => new
                {
                    ChannelId = table.Column<Guid>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Channels", x => x.ChannelId);
                    table.ForeignKey(
                        name: "FK_Channels_Groups_GroupId",
                        column: x => x.GroupId,
                        principalSchema: "Groups",
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChannelMemberPermissionOverrides",
                schema: "Groups",
                columns: table => new
                {
                    Permission = table.Column<string>(nullable: false),
                    UserIdOfMember = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    ChannelId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelMemberPermissionOverrides", x => new { x.UserIdOfMember, x.Permission });
                    table.ForeignKey(
                        name: "FK_ChannelMemberPermissionOverrides_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalSchema: "Groups",
                        principalTable: "Channels",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChannelRolePermissionOverrides",
                schema: "Groups",
                columns: table => new
                {
                    Permission = table.Column<string>(nullable: false),
                    RoleId = table.Column<Guid>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    ChannelId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelRolePermissionOverrides", x => new { x.RoleId, x.Permission });
                    table.ForeignKey(
                        name: "FK_ChannelRolePermissionOverrides_Channels_ChannelId",
                        column: x => x.ChannelId,
                        principalSchema: "Groups",
                        principalTable: "Channels",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMemberPermissionOverrides_ChannelId",
                schema: "Groups",
                table: "ChannelMemberPermissionOverrides",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelRolePermissionOverrides_ChannelId",
                schema: "Groups",
                table: "ChannelRolePermissionOverrides",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_Channels_GroupId",
                schema: "Groups",
                table: "Channels",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelMemberPermissionOverrides",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "ChannelRolePermissionOverrides",
                schema: "Groups");

            migrationBuilder.DropTable(
                name: "Channels",
                schema: "Groups");
        }
    }
}
