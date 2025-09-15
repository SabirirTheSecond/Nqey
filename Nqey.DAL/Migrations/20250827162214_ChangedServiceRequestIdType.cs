using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ChangedServiceRequestIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. Drop the old PK
            migrationBuilder.DropPrimaryKey(
                name: "PK_ServicesRequests",
                table: "ServicesRequests");

            // 2. Drop the old column
            migrationBuilder.DropColumn(
                name: "ServiceRequestId",
                table: "ServicesRequests");

            // 3. Add the new column as INT IDENTITY
            migrationBuilder.AddColumn<int>(
                name: "ServiceRequestId",
                table: "ServicesRequests",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // 4. Re-add PK on the new column
            migrationBuilder.AddPrimaryKey(
                name: "PK_ServicesRequests",
                table: "ServicesRequests",
                column: "ServiceRequestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the steps for rollback

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServicesRequests",
                table: "ServicesRequests");

            migrationBuilder.DropColumn(
                name: "ServiceRequestId",
                table: "ServicesRequests");

            migrationBuilder.AddColumn<string>(
                name: "ServiceRequestId",
                table: "ServicesRequests",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServicesRequests",
                table: "ServicesRequests",
                column: "ServiceRequestId");
        }

    }
}
