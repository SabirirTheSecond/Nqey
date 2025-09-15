using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddingMessageDbset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_RecieverId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_SenderId",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Messages");

            migrationBuilder.RenameIndex(
                name: "IX_Message_SenderId_RecieverId_TimeStamp",
                table: "Messages",
                newName: "IX_Messages_SenderId_RecieverId_TimeStamp");

            migrationBuilder.RenameIndex(
                name: "IX_Message_RecieverId_TimeStamp",
                table: "Messages",
                newName: "IX_Messages_RecieverId_TimeStamp");

            migrationBuilder.RenameIndex(
                name: "IX_Message_RecieverId_IsRead_TimeStamp",
                table: "Messages",
                newName: "IX_Messages_RecieverId_IsRead_TimeStamp");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_RecieverId",
                table: "Messages",
                column: "RecieverId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_RecieverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Message");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SenderId_RecieverId_TimeStamp",
                table: "Message",
                newName: "IX_Message_SenderId_RecieverId_TimeStamp");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_RecieverId_TimeStamp",
                table: "Message",
                newName: "IX_Message_RecieverId_TimeStamp");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_RecieverId_IsRead_TimeStamp",
                table: "Message",
                newName: "IX_Message_RecieverId_IsRead_TimeStamp");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Users_RecieverId",
                table: "Message",
                column: "RecieverId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Users_SenderId",
                table: "Message",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
