using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Model.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Helper
{
    public static class InstallDBHelper
    {
        #region Install  Context
        public static IServiceCollection InstallIdentityContext(this IServiceCollection services)
        {
            services.AddDbContext<IdentityContext>();
            return services;
        }
         public static IServiceCollection InstallAuditLogContext(this IServiceCollection services)
        {
            services.AddDbContext<AuditLogContext>();
            return services;
        }
          public static IServiceCollection InstallWarehouseContext(this IServiceCollection services)
        {
            services.AddDbContext<WarehouseContext>();
            return services;
        }
          public static IServiceCollection InstallRetailContext(this IServiceCollection services)
        {
            services.AddDbContext<RetailContext>();
            return services;
        }
        #endregion

        #region Install  Context Factory
        public static IServiceCollection ContructIdentityContext(this IServiceCollection services)
        {
            services.AddDbContextFactory<IdentityContext>();
            return services;
        }
         public static IServiceCollection ContructInstallAuditLogContext(this IServiceCollection services)
        {
            services.AddDbContextFactory<AuditLogContext>();
            return services;
        }
          public static IServiceCollection ContructWarehouseContext(this IServiceCollection services)
        {
            services.AddDbContextFactory<WarehouseContext>();
            return services;
        }
          public static IServiceCollection ContructRetailContext(this IServiceCollection services)
        {
            services.AddDbContextFactory<RetailContext>();
            return services;
        }
        #endregion


        #region install Database
        public static IServiceCollection InstallDatabase<TContext> (this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>();
            return services;
        }
        public static IServiceCollection InstallDatabase<TContext1, TContext2> (this IServiceCollection services)
            where TContext1 : DbContext
            where TContext2 : DbContext
        {
            services.AddDbContext<TContext1>();
            services.AddDbContext<TContext2>();
            return services;
        }
        public static IServiceCollection InstallDatabase<TContext1, TContext2,TContext3> (this IServiceCollection services)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
        {
            services.AddDbContext<TContext1>();
            services.AddDbContext<TContext2>();
            services.AddDbContext<TContext3>();
            return services;
        }
        public static IServiceCollection InstallDatabase<TContext1, TContext2,TContext3,TContext4> (this IServiceCollection services)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
            where TContext4 : DbContext
        {
            services.AddDbContext<TContext1>();
            services.AddDbContext<TContext2>();
            services.AddDbContext<TContext3>();
            services.AddDbContext<TContext4>();
            return services;
        }
         public static IServiceCollection InstallDatabase<TContext1, TContext2,TContext3,TContext4,TContext5> (this IServiceCollection services)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
            where TContext4 : DbContext
            where TContext5 : DbContext
        {
            services.AddDbContext<TContext1>();
            services.AddDbContext<TContext2>();
            services.AddDbContext<TContext3>();
            services.AddDbContext<TContext4>();
            services.AddDbContext<TContext5>();
            return services;
        }
         public static IServiceCollection InstallDatabase<TContext1, TContext2,TContext3,TContext4,TContext5,TContext6> (this IServiceCollection services)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
            where TContext4 : DbContext
            where TContext5 : DbContext
            where TContext6 : DbContext
        {
            services.AddDbContext<TContext1>();
            services.AddDbContext<TContext2>();
            services.AddDbContext<TContext3>();
            services.AddDbContext<TContext4>();
            services.AddDbContext<TContext5>();
            services.AddDbContext<TContext6>();
            return services;
        }
        #endregion

        #region install Database Factory
        public static IServiceCollection ContructDatabase<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddDbContextFactory<TContext>();
            return services;
        }
        public static IServiceCollection ContructDatabase<TContext1, TContext2>(this IServiceCollection services)
            where TContext1 : DbContext
            where TContext2 : DbContext
        {
            services.AddDbContextFactory<TContext1>();
            services.AddDbContextFactory<TContext2>();
            return services;
        }
        public static IServiceCollection ContructDatabase<TContext1, TContext2, TContext3>(this IServiceCollection services)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
        {
            services.AddDbContextFactory<TContext1>();
            services.AddDbContextFactory<TContext2>();
            services.AddDbContextFactory<TContext3>();
            return services;
        }
        public static IServiceCollection ContructDatabase<TContext1, TContext2, TContext3, TContext4>(this IServiceCollection services)
            where TContext1 : DbContext
            where TContext2 : DbContext
            where TContext3 : DbContext
            where TContext4 : DbContext
        {
            services.AddDbContextFactory<TContext1>();
            services.AddDbContextFactory<TContext2>();
            services.AddDbContextFactory<TContext3>();
            services.AddDbContextFactory<TContext4>();
            return services;
        }
        public static IServiceCollection ContructDatabase<TContext1, TContext2, TContext3, TContext4, TContext5>(this IServiceCollection services)
           where TContext1 : DbContext
           where TContext2 : DbContext
           where TContext3 : DbContext
           where TContext4 : DbContext
           where TContext5 : DbContext
        {
            services.AddDbContextFactory<TContext1>();
            services.AddDbContextFactory<TContext2>();
            services.AddDbContextFactory<TContext3>();
            services.AddDbContextFactory<TContext4>();
            services.AddDbContextFactory<TContext5>();
            return services;
        }
        public static IServiceCollection ContructDatabase<TContext1, TContext2, TContext3, TContext4, TContext5, TContext6>(this IServiceCollection services)
           where TContext1 : DbContext
           where TContext2 : DbContext
           where TContext3 : DbContext
           where TContext4 : DbContext
           where TContext5 : DbContext
           where TContext6 : DbContext
        {
            services.AddDbContextFactory<TContext1>();
            services.AddDbContextFactory<TContext2>();
            services.AddDbContextFactory<TContext3>();
            services.AddDbContextFactory<TContext4>();
            services.AddDbContextFactory<TContext5>();
            services.AddDbContextFactory<TContext6>();
            return services;
        }
        #endregion
    }
}
