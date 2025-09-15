using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedMessagingStructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Clients_ClientUserId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Clients_ClientUserId1",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Providers_ProviderUserId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Providers_ProviderUserId1",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ClientUserId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ClientUserId1",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ProviderUserId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_ProviderUserId1",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ClientUserId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ClientUserId1",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ProviderUserId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ProviderUserId1",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Message",
                newName: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_RecieverId_IsRead_TimeStamp",
                table: "Message",
                columns: new[] { "RecieverId", "IsRead", "TimeStamp" });

            migrationBuilder.CreateIndex(
                name: "IX_Message_RecieverId_TimeStamp",
                table: "Message",
                columns: new[] { "RecieverId", "TimeStamp" },
                filter: "[IsRead]=0");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId_RecieverId_TimeStamp",
                table: "Message",
                columns: new[] { "SenderId", "RecieverId", "TimeStamp" });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_RecieverId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_SenderId",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_RecieverId_IsRead_TimeStamp",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_RecieverId_TimeStamp",
                table: "Message");

            migrationBuilder.DropIndex(
                name: "IX_Message_SenderId_RecieverId_TimeStamp",
                table: "Message");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "Message",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "ClientUserId",
                table: "Message",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClientUserId1",
                table: "Message",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProviderUserId",
                table: "Message",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProviderUserId1",
                table: "Message",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Message_ClientUserId",
                table: "Message",
                column: "ClientUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ClientUserId1",
                table: "Message",
                column: "ClientUserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ProviderUserId",
                table: "Message",
                column: "ProviderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_ProviderUserId1",
                table: "Message",
                column: "ProviderUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Clients_ClientUserId",
                table: "Message",
                column: "ClientUserId",
                principalTable: "Clients",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Clients_ClientUserId1",
                table: "Message",
                column: "ClientUserId1",
                principalTable: "Clients",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Providers_ProviderUserId",
                table: "Message",
                column: "ProviderUserId",
                principalTable: "Providers",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Providers_ProviderUserId1",
                table: "Message",
                column: "ProviderUserId1",
                principalTable: "Providers",
                principalColumn: "UserId");
        }
    }
}
