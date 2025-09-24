using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserAnalyticsToUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserAnalytics_ComplaintsAgainstCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserAnalytics_FiledComplaintsCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnalyticVariables_Bookings",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnalyticVariables_Cancelations",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnalyticVariables_ComplaintsAgainstCount",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AnalyticVariables_FiledComplaintsCount",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserAnalytics_ComplaintsAgainstCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserAnalytics_FiledComplaintsCount",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AnalyticVariables_Bookings",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "AnalyticVariables_Cancelations",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "AnalyticVariables_ComplaintsAgainstCount",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "AnalyticVariables_FiledComplaintsCount",
                table: "Clients");
        }
    }
}
