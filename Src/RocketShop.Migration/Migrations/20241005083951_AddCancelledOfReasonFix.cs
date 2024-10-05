using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RocketShop.Migration.Migrations
{
    /// <inheritdoc />
    public partial class AddCancelledOfReasonFix : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CancelledReason",
                table: "Payroll",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelledReason",
                table: "Payroll");
        }
    }
}
