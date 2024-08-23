using Microsoft.EntityFrameworkCore.Migrations;
using RocketShop.Database;

#nullable disable

namespace RocketShop.Migration.Migrations
{
    /// <inheritdoc />
    public partial class Rename_Provident_Currency : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        readonly CreateViewSql createView = CreateView.UserFinacialViewV4;
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currenct",
                table: "ProvidentFund",
                newName: "Currency");
            migrationBuilder.Sql($"Drop View \"{TableConstraint.UserFinacialView}\"");
            migrationBuilder.Sql(createView.Up);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"Drop View \"{TableConstraint.UserFinacialView}\"");
            migrationBuilder.Sql(createView.Down);
            migrationBuilder.RenameColumn(
                name: "Currency",
                table: "ProvidentFund",
                newName: "Currenct");
        }
    }
}
