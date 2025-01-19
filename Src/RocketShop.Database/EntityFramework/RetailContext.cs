using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Retail;
using RocketShop.Database.Model.Retail.SubModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.EntityFramework
{
    public class RetailContext(IConfiguration configuration) : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            string connectionString = configuration.GetRetailConnectionString();
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("RocketShop.Migration"));
            var enableLog = configuration.GetSection("EnabledSqlLogging").Get<bool>();
            if (enableLog)
                options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MainCategory>().HasIndex(x =>  x.NameEn).IsUnique();
            builder.Entity<MainCategory>().HasIndex(x =>  x.NameTh).IsUnique();
            builder.Entity<SubCategory>().HasIndex(x => new { x.NameEn, x.NameTh,x.MainCategoryId });
            builder.Entity<Product>().HasIndex(x => new { x.SubCategoryId, x.Name });
            builder.Entity<MainCategory>().Property(f => f.Id).ValueGeneratedOnAdd();
            builder.Entity<SubCategory>().Property(f => f.Id).ValueGeneratedOnAdd();
        }

        public required DbSet<MainCategory> MainCategories { get; set; }
        public required DbSet<SubCategory> SubCategories { get; set; }
        public required DbSet<Product> Products { get; set; }
    }
}
