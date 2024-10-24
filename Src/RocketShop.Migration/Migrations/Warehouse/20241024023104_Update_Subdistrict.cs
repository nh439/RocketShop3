using Microsoft.EntityFrameworkCore.Migrations;
using RocketShop.Database;

#nullable disable

namespace RocketShop.Migration.Migrations.Warehouse
{
    /// <inheritdoc />
    public partial class Update_Subdistrict : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NameTH",
                table: "SubDistricts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "NameEN",
                table: "SubDistricts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AlterColumn<string>(
                name: "NameTH",
                table: "SubDistricts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "NameEN",
                table: "SubDistricts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }
    }
}
