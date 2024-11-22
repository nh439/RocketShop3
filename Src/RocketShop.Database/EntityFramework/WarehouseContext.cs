using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.Warehouse;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.Model.Warehouse.Views;
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
            builder.Entity<AllowedObject>().HasIndex(i => new { i.Client, i.ObjectName });
            builder.Entity<ClientSecret>().HasIndex(i => i.Client);
            builder.Entity<Token>().HasIndex(i => i.Client);
            builder.Entity<Client>().HasIndex(i => i.ClientId).IsUnique();
            builder.Entity<Client>().HasIndex(i => i.ClientName).IsUnique();
            builder.Entity<AddressView>()
                .HasNoKey().ToView(TableConstraint.AddressView);

            builder.Entity<ClientHistory>().HasIndex(i => i.ClientId);
            builder.Entity<ClientHistory>().HasIndex(i => i.Key);
        }

        public DbSet<Province> Province { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<SubDistrict> SubDistrict { get; set; }
        
        public DbSet<Client> Client {  get; set; }
        public DbSet<ClientSecret> ClientSecret { get; set; }
        public DbSet<AllowedObject> AllowedObject { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<ClientHistory> ClientHistory { get; set; }

        //Views
        public virtual DbSet<AddressView> AddressView { get; set; }
    }
}
