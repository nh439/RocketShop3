using Microsoft.EntityFrameworkCore.Migrations;
using RocketShop.Database;

#nullable disable

namespace RocketShop.Migration.Migrations
{
    /// <inheritdoc />
    public partial class Add_UserViews : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(CreateView.UserView.Up);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(CreateView.UserView.Down);
        }
    }
}
