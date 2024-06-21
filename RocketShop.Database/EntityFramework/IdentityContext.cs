using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.Identity.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RocketShop.Database.EntityFramework
{
    public class IdentityContext : IdentityDbContext<User, IdentityRole, string>
    {
        readonly IConfiguration configuration;
        public IdentityContext(IConfiguration configuration) : base()
        {
            this.configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            string connectionString = configuration.GetConnectionString("IdentityDatabase")!;
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("RocketShop.Migration"));
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(x => x.EmployeeCode)
                .IsUnique();
            builder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();
            builder.Entity<UserInformation>()
                .HasIndex(x => x.ManagerId);

            builder.Entity<UserView>()
                .HasNoKey()
                .ToView(TableConstraint.UserView);

            builder.Entity<UserRole>().HasKey(k => new {k.RoleId, k.UserId});

            builder.Entity<Role>()
                .HasData(new RocketShop.Database.Model.Identity.Role
                {
                    Id=1,
                    RoleName = "Application Starter"
                });

            base.OnModelCreating(builder);
        }
        public DbSet<UserInformation> UserInformation { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Role> Role { get; set; }

        public virtual DbSet<UserView> UserViews { get; set; }
    }
}
