using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RocketShop.Migration.Migrations.Retail
{
    /// <inheritdoc />
    public partial class Add_Unique_For_Main_Category : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_M_Category_NameEn_NameTh",
                table: "M_Category");

            migrationBuilder.AddColumn<bool>(
                name: "Primary",
                table: "S_Category",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_M_Category_NameEn",
                table: "M_Category",
                column: "NameEn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_M_Category_NameTh",
                table: "M_Category",
                column: "NameTh",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_M_Category_NameEn",
                table: "M_Category");

            migrationBuilder.DropIndex(
                name: "IX_M_Category_NameTh",
                table: "M_Category");

            migrationBuilder.DropColumn(
                name: "Primary",
                table: "S_Category");

            migrationBuilder.CreateIndex(
                name: "IX_M_Category_NameEn_NameTh",
                table: "M_Category",
                columns: new[] { "NameEn", "NameTh" });
        }
    }
}
