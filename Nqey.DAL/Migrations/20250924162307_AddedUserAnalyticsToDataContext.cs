using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nqey.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserAnalyticsToDataContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AnalyticalVariables_Refuses",
                table: "Providers",
                newName: "ProviderAnalytics_Refuses");

            migrationBuilder.RenameColumn(
                name: "AnalyticalVariables_JobsDone",
                table: "Providers",
                newName: "ProviderAnalytics_JobsDone");

            migrationBuilder.RenameColumn(
                name: "AnalyticalVariables_FiledComplaintsCount",
                table: "Providers",
                newName: "ProviderAnalytics_FiledComplaintsCount");

            migrationBuilder.RenameColumn(
                name: "AnalyticalVariables_Completions",
                table: "Providers",
                newName: "ProviderAnalytics_Completions");

            migrationBuilder.RenameColumn(
                name: "AnalyticalVariables_ComplaintsAgainstCount",
                table: "Providers",
                newName: "ProviderAnalytics_ComplaintsAgainstCount");

            migrationBuilder.RenameColumn(
                name: "AnalyticalVariables_Accepts",
                table: "Providers",
                newName: "ProviderAnalytics_Accepts");

            migrationBuilder.RenameColumn(
                name: "AnalyticVariables_FiledComplaintsCount",
                table: "Clients",
                newName: "ClientAnalytics_FiledComplaintsCount");

            migrationBuilder.RenameColumn(
                name: "AnalyticVariables_ComplaintsAgainstCount",
                table: "Clients",
                newName: "ClientAnalytics_ComplaintsAgainstCount");

            migrationBuilder.RenameColumn(
                name: "AnalyticVariables_Cancelations",
                table: "Clients",
                newName: "ClientAnalytics_Cancelations");

            migrationBuilder.RenameColumn(
                name: "AnalyticVariables_Bookings",
                table: "Clients",
                newName: "ClientAnalytics_Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "UserAnalytics_FiledComplaintsCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "UserAnalytics_ComplaintsAgainstCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientAnalytics_FiledComplaintsCount",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientAnalytics_ComplaintsAgainstCount",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientAnalytics_Cancelations",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientAnalytics_Bookings",
                table: "Clients",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProviderAnalytics_Refuses",
                table: "Providers",
                newName: "AnalyticalVariables_Refuses");

            migrationBuilder.RenameColumn(
                name: "ProviderAnalytics_JobsDone",
                table: "Providers",
                newName: "AnalyticalVariables_JobsDone");

            migrationBuilder.RenameColumn(
                name: "ProviderAnalytics_FiledComplaintsCount",
                table: "Providers",
                newName: "AnalyticalVariables_FiledComplaintsCount");

            migrationBuilder.RenameColumn(
                name: "ProviderAnalytics_Completions",
                table: "Providers",
                newName: "AnalyticalVariables_Completions");

            migrationBuilder.RenameColumn(
                name: "ProviderAnalytics_ComplaintsAgainstCount",
                table: "Providers",
                newName: "AnalyticalVariables_ComplaintsAgainstCount");

            migrationBuilder.RenameColumn(
                name: "ProviderAnalytics_Accepts",
                table: "Providers",
                newName: "AnalyticalVariables_Accepts");

            migrationBuilder.RenameColumn(
                name: "ClientAnalytics_FiledComplaintsCount",
                table: "Clients",
                newName: "AnalyticVariables_FiledComplaintsCount");

            migrationBuilder.RenameColumn(
                name: "ClientAnalytics_ComplaintsAgainstCount",
                table: "Clients",
                newName: "AnalyticVariables_ComplaintsAgainstCount");

            migrationBuilder.RenameColumn(
                name: "ClientAnalytics_Cancelations",
                table: "Clients",
                newName: "AnalyticVariables_Cancelations");

            migrationBuilder.RenameColumn(
                name: "ClientAnalytics_Bookings",
                table: "Clients",
                newName: "AnalyticVariables_Bookings");

            migrationBuilder.AlterColumn<int>(
                name: "UserAnalytics_FiledComplaintsCount",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "UserAnalytics_ComplaintsAgainstCount",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AnalyticVariables_FiledComplaintsCount",
                table: "Clients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AnalyticVariables_ComplaintsAgainstCount",
                table: "Clients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AnalyticVariables_Cancelations",
                table: "Clients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "AnalyticVariables_Bookings",
                table: "Clients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);
        }
    }
}
