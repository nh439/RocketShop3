using Microsoft.EntityFrameworkCore.Migrations;
using RocketShop.Database;

#nullable disable

namespace RocketShop.Migration.Migrations
{
    /// <inheritdoc />
    public partial class Add_Lock_Until :Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        readonly CreateViewSql createView = CreateView.UserViewV3;
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"Drop View \"{TableConstraint.UserView}\"");
            migrationBuilder.Sql(createView.Up);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"Drop View \"{TableConstraint.UserView}\"");
            migrationBuilder.Sql(createView.Down);
        }
    }
}
