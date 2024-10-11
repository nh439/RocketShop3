using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RocketShop.Database.Extension;
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

            string connectionString = configuration.GetIdentityConnectionString();
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly("RocketShop.Migration"));
            var enableLog = configuration.GetSection("EnabledSqlLogging").Get<bool>();
            if (enableLog)
                options.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasIndex(x => x.EmployeeCode)
                .IsUnique();
            builder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();
            builder.Entity<User>()
               .HasIndex(x => x.ProviderName);
             builder.Entity<User>()
               .HasIndex(x => x.ProviderKey);

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

            builder.Entity<ChangePasswordHistory>().HasIndex(x => x.UserId);
            builder.Entity<ChangePasswordHistory>().HasIndex(x => x.Reset);
            builder.Entity<UserFinancal>().HasIndex(x => x.UserId);
            builder.Entity<UserAddiontialExpense>().HasIndex(x => x.UserId);
            builder.Entity<UserPayroll>().HasIndex(x => x.UserId);
            builder.Entity<UserPayroll>().HasIndex(x => x.Cancelled);
            builder.Entity<AdditionalPayroll>().HasIndex(x => x.PayrollId);
            builder.Entity<UserFinancialView>().HasNoKey().ToView(TableConstraint.UserFinacialView);
            base.OnModelCreating(builder);
        }
        public DbSet<UserInformation> UserInformation { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<ChangePasswordHistory> ChangePasswordHistory { get; set; }
        public DbSet<UserFinancal> UserFinancal { get; set; }
        public DbSet<UserAddiontialExpense> UserAddiontialExpense { get; set; }
        public DbSet<UserProvidentFund> UserProvidentFund { get; set; }
        public DbSet<UserPayroll> UserPayroll { get; set; }
        public DbSet<AdditionalPayroll> AdditionalPayroll { get; set; }

        public virtual DbSet<UserView> UserViews { get; set; }
        public virtual DbSet<UserFinancialView> UserFinancialView { get; set; }
    }
}
