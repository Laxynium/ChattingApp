using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InstantMessenger.PrivateMessages.Infrastructure.Database.Migrations
{
    public partial class AddForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "To",
                schema: "PrivateMessages",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "From",
                schema: "PrivateMessages",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ConversationId",
                schema: "PrivateMessages",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SecondParticipant",
                schema: "PrivateMessages",
                table: "Conversations",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "FirstParticipant",
                schema: "PrivateMessages",
                table: "Conversations",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                schema: "PrivateMessages",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                schema: "PrivateMessages",
                table: "Messages",
                column: "ConversationId",
                principalSchema: "PrivateMessages",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                schema: "PrivateMessages",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ConversationId",
                schema: "PrivateMessages",
                table: "Messages");

            migrationBuilder.AlterColumn<Guid>(
                name: "To",
                schema: "PrivateMessages",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "From",
                schema: "PrivateMessages",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "ConversationId",
                schema: "PrivateMessages",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "SecondParticipant",
                schema: "PrivateMessages",
                table: "Conversations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "FirstParticipant",
                schema: "PrivateMessages",
                table: "Conversations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid));
        }
    }
}
