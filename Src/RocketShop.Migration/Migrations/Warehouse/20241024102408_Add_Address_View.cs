using Microsoft.EntityFrameworkCore.Migrations;
using RocketShop.Database;

#nullable disable

namespace RocketShop.Migration.Migrations.Warehouse
{
    /// <inheritdoc />
    public partial class Add_Address_View : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        static CreateViewSql createView = CreateView.CreateAddrView;
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(createView.Up);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(createView.Down);
        }
    }
}
