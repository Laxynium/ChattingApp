using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.Groups.Infrastructure.Database.Migrations
{
    public partial class FixKeyForOverrides : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChannelRolePermissionOverrides",
                schema: "Groups",
                table: "ChannelRolePermissionOverrides");

            migrationBuilder.DropIndex(
                name: "IX_ChannelRolePermissionOverrides_ChannelId",
                schema: "Groups",
                table: "ChannelRolePermissionOverrides");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChannelMemberPermissionOverrides",
                schema: "Groups",
                table: "ChannelMemberPermissionOverrides");

            migrationBuilder.DropIndex(
                name: "IX_ChannelMemberPermissionOverrides_ChannelId",
                schema: "Groups",
                table: "ChannelMemberPermissionOverrides");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChannelRolePermissionOverrides",
                schema: "Groups",
                table: "ChannelRolePermissionOverrides",
                columns: new[] { "ChannelId", "RoleId", "Permission" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChannelMemberPermissionOverrides",
                schema: "Groups",
                table: "ChannelMemberPermissionOverrides",
                columns: new[] { "ChannelId", "UserIdOfMember", "Permission" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChannelRolePermissionOverrides",
                schema: "Groups",
                table: "ChannelRolePermissionOverrides");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChannelMemberPermissionOverrides",
                schema: "Groups",
                table: "ChannelMemberPermissionOverrides");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChannelRolePermissionOverrides",
                schema: "Groups",
                table: "ChannelRolePermissionOverrides",
                columns: new[] { "RoleId", "Permission" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChannelMemberPermissionOverrides",
                schema: "Groups",
                table: "ChannelMemberPermissionOverrides",
                columns: new[] { "UserIdOfMember", "Permission" });

            migrationBuilder.CreateIndex(
                name: "IX_ChannelRolePermissionOverrides_ChannelId",
                schema: "Groups",
                table: "ChannelRolePermissionOverrides",
                column: "ChannelId");

            migrationBuilder.CreateIndex(
                name: "IX_ChannelMemberPermissionOverrides_ChannelId",
                schema: "Groups",
                table: "ChannelMemberPermissionOverrides",
                column: "ChannelId");
        }
    }
}
