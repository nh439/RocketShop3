using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RocketShop.Migration.Migrations
{
    /// <inheritdoc />
    public partial class Add_User_Finance : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddiontialExpense",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    OneTimePay = table.Column<bool>(type: "boolean", nullable: false),
                    Month = table.Column<int>(type: "integer", nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    PreiodType = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    Paid = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddiontialExpense", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AdditionalPayroll",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    PayrollId = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalPayroll", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payroll",
                columns: table => new
                {
                    PayRollId = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    PayrollDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Salary = table.Column<decimal>(type: "numeric", nullable: false),
                    SocialSecurites = table.Column<decimal>(type: "numeric", nullable: false),
                    ProvidentFund = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPayment = table.Column<decimal>(type: "numeric", nullable: false),
                    TravelExpenses = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalAdditionalPay = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payroll", x => x.PayRollId);
                });

            migrationBuilder.CreateTable(
                name: "ProvidentFund",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    Currenct = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProvidentFund", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "UserFinancal",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Salary = table.Column<decimal>(type: "numeric", nullable: false),
                    SocialSecurites = table.Column<decimal>(type: "numeric", nullable: false),
                    ProvidentFund = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalPayment = table.Column<decimal>(type: "numeric", nullable: false),
                    TravelExpenses = table.Column<decimal>(type: "numeric", nullable: false),
                    TotalAddiontialExpense = table.Column<decimal>(type: "numeric", nullable: false),
                    BanckName = table.Column<string>(type: "text", nullable: false),
                    AccountNo = table.Column<string>(type: "text", nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserFinancal", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddiontialExpense_UserId",
                table: "AddiontialExpense",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalPayroll_PayrollId",
                table: "AdditionalPayroll",
                column: "PayrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Payroll_UserId",
                table: "Payroll",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserFinancal_UserId",
                table: "UserFinancal",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddiontialExpense");

            migrationBuilder.DropTable(
                name: "AdditionalPayroll");

            migrationBuilder.DropTable(
                name: "Payroll");

            migrationBuilder.DropTable(
                name: "ProvidentFund");

            migrationBuilder.DropTable(
                name: "UserFinancal");
        }
    }
}
