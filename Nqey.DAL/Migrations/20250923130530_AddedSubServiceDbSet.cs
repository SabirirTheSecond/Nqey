using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedSubServiceDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubService_Providers_ProviderUserId",
                table: "SubService");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubService",
                table: "SubService");

            migrationBuilder.RenameTable(
                name: "SubService",
                newName: "SubServices");

            migrationBuilder.RenameIndex(
                name: "IX_SubService_ProviderUserId",
                table: "SubServices",
                newName: "IX_SubServices_ProviderUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubServices",
                table: "SubServices",
                column: "SubServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubServices_Providers_ProviderUserId",
                table: "SubServices",
                column: "ProviderUserId",
                principalTable: "Providers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubServices_Providers_ProviderUserId",
                table: "SubServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubServices",
                table: "SubServices");

            migrationBuilder.RenameTable(
                name: "SubServices",
                newName: "SubService");

            migrationBuilder.RenameIndex(
                name: "IX_SubServices_ProviderUserId",
                table: "SubService",
                newName: "IX_SubService_ProviderUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubService",
                table: "SubService",
                column: "SubServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubService_Providers_ProviderUserId",
                table: "SubService",
                column: "ProviderUserId",
                principalTable: "Providers",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
