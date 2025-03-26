using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VacationManagmentApplication.Data.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HR",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HR", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BonusDays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HRId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BonusDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BonusDays_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BonusDays_HR_HRId",
                        column: x => x.HRId,
                        principalTable: "HR",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MedicalCares",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InvoicePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HRId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalCares", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalCares_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MedicalCares_HR_HRId",
                        column: x => x.HRId,
                        principalTable: "HR",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "YearlyVacations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HRId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearlyVacations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YearlyVacations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_YearlyVacations_HR_HRId",
                        column: x => x.HRId,
                        principalTable: "HR",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BonusDays_EmployeeId",
                table: "BonusDays",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BonusDays_HRId",
                table: "BonusDays",
                column: "HRId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalCares_EmployeeId",
                table: "MedicalCares",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalCares_HRId",
                table: "MedicalCares",
                column: "HRId");

            migrationBuilder.CreateIndex(
                name: "IX_YearlyVacations_EmployeeId",
                table: "YearlyVacations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_YearlyVacations_HRId",
                table: "YearlyVacations",
                column: "HRId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BonusDays");

            migrationBuilder.DropTable(
                name: "MedicalCares");

            migrationBuilder.DropTable(
                name: "YearlyVacations");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "HR");
        }
    }
}
