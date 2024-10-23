using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Warehouse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.EntityFramework
{
    public class WarehouseContext(IConfiguration configuration) : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            string connectionString = configuration.GetWarehouseConnectionString();
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("RocketShop.Migration"));
            var enableLog = configuration.GetSection("EnabledSqlLogging").Get<bool>();
            if (enableLog)
                options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Province>().HasIndex(i=>i.Code).IsUnique();
            builder.Entity<District>().HasIndex(i=>i.Code).IsUnique();
            builder.Entity<District>().HasIndex(i => i.ProvinceId);
            builder.Entity<SubDistrict>().HasIndex(i=>i.Code).IsUnique();
            builder.Entity<SubDistrict>().HasIndex(i => i.PostalCode);
            builder.Entity<SubDistrict>().HasIndex(i => i.DistrictId);
        }

        public DbSet<Province> Province { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<SubDistrict> SubDistrict { get; set; }
    }
}
