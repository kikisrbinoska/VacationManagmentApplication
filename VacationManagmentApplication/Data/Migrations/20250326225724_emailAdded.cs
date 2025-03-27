using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationManagmentApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class emailAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "HR",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Employees",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "HR");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Employees");
        }
    }
}
