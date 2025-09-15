using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedServiceRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Services_ServiceId",
                table: "Providers");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "Providers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "ServicesRequests",
                columns: table => new
                {
                    ServiceRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProviderUserId = table.Column<int>(type: "int", nullable: true),
                    ServiceRequestStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicesRequests", x => x.ServiceRequestId);
                    table.ForeignKey(
                        name: "FK_ServicesRequests_Providers_ProviderUserId",
                        column: x => x.ProviderUserId,
                        principalTable: "Providers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServicesRequests_ProviderUserId",
                table: "ServicesRequests",
                column: "ProviderUserId",
                unique: true,
                filter: "[ProviderUserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Services_ServiceId",
                table: "Providers",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Providers_Services_ServiceId",
                table: "Providers");

            migrationBuilder.DropTable(
                name: "ServicesRequests");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceId",
                table: "Providers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Providers_Services_ServiceId",
                table: "Providers",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ServiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
