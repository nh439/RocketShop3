using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RocketShop.Database.Extension;
using RocketShop.Database.Model.AuditLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.EntityFramework
{
    public class AuditLogContext(IConfiguration configuration) : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            string connectionString = configuration.GetAuditLogConnectionString();
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("RocketShop.Migration"));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ActivityLog>().HasIndex(x => x.ServiceName);
            builder.Entity<ActivityLog>().HasIndex(x => x.LogDate);
            builder.Entity<ActivityLog>().HasIndex(x => x.Actor);
            builder.Entity<ActivityLog>().HasIndex(x => x.ActorName);
        }
        public DbSet<ActivityLog> ActivityLog {  get; set; }
    }
}
