using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class NullableProviderReviewId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Review_ReviewId",
                table: "Providers");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "Providers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Review_ReviewId",
                table: "Providers",
                column: "ReviewId",
                principalTable: "Review",
                principalColumn: "ReviewId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Review_ReviewId",
                table: "Providers");

            migrationBuilder.AlterColumn<int>(
                name: "ReviewId",
                table: "Providers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Review_ReviewId",
                table: "Providers",
                column: "ReviewId",
                principalTable: "Review",
                principalColumn: "ReviewId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
