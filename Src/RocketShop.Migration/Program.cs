
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Helper;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.NonEntityFramework;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Helper;
using RocketShop.Migration.Configuration;

namespace RocketShop.Migration
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.InstallConfiguration();

            var startUser = configuration.GetSection("StartUser").Get<StartUserConfiguration>();
            builder.InstallServices(service =>
            {
                service.AddIdentity<User, IdentityRole>(option =>
                {
                    option.SignIn.RequireConfirmedAccount = false;
                    option.Password.RequireNonAlphanumeric = false;
                }).AddEntityFrameworkStores<IdentityContext>()
.AddDefaultTokenProviders();
                service.AddIdentityCore<User>(s =>
                {
                })
    .AddRoles<IdentityRole>()
    .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, IdentityRole>>()
    .AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders();
                service.InstallDatabase<IdentityContext,AuditLogContext,WarehouseContext>()
                .AddAuthorization()
                .AddEndpointsApiExplorer()
                .AddSwaggerGen();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            var flowResult = await app.Services.InvokeEntryFlowAndReturn(async scope =>
            {
                try
                {
                    
                    Console.WriteLine("Start Migrate");
                    var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();
                    var auditContext = scope.ServiceProvider.GetRequiredService<AuditLogContext>();
                    var warehouseContext = scope.ServiceProvider.GetRequiredService<WarehouseContext>();
                    var whConnStr = warehouseContext.Database.GetConnectionString();
                    var otherConnStr = new[]
                    {
                        auditContext.Database.GetConnectionString(),
                      context.Database.GetConnectionString()
                    };
                    if(otherConnStr.Where(x=>x == whConnStr).HasData())
                    {
                        Console.WriteLine("Do not share the warehouse database with other databases.");
                        return -2;
                    }
                    context!.Database.Migrate();
                    Console.WriteLine("Identity Migrate Success");
                    auditContext!.Database.Migrate();
                    Console.WriteLine("Audit Log Migrate Success");
                     warehouseContext!.Database.Migrate();
                    Console.WriteLine("Warehouse Migrate Success");
                    Console.WriteLine("End Migrate");
                    Console.WriteLine("Create First User");
                    using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var exists = await userManager.FindByEmailAsync(startUser!.Email);
                    string userId = string.Empty;
                    if (exists.IsNull())
                    {
                        var user = new User
                        {
                            Email = startUser.Email,
                            AccessFailedCount = 0,
                            CreateDate = DateTime.UtcNow,
                            EmailVerified = true,
                            EmployeeCode = startUser.EmployeeCode,
                            Firstname = "Rocket",
                            UserName = startUser.Username,
                            Surname = "Admin",
                            Resigned = false,
                            CreateBy = "SYSTEM"
                        };
                        await userManager.CreateAsync(user, startUser.Password);

                        context.UserInformation.Add(new UserInformation
                        {
                            BrithDay = DateTime.UtcNow,
                            StartWorkDate = DateTime.UtcNow,
                            UserId = user.Id,
                            CreateBy="SYSTEM"
                        });

                        await context.SaveChangesAsync();
                        userId = user.Id;
                        Console.WriteLine("First User Create Successful");
                    }
                    else
                    {
                        userId = exists!.Id;
                        Console.WriteLine("First User Already Exists Skipped");
                    }
                    var hasStarterRole = await context.UserRole.AnyAsync(x => x.UserId == userId && x.RoleId == 1);
                    if (!hasStarterRole)
                    {
                        var newRole = new UserRole
                        {
                            RoleId = 1,
                            UserId = userId
                        };
                        context.UserRole.Add(newRole);
                        await context.SaveChangesAsync();
                    }

                    for (int i = 10; i > 0; i--)
                    {
                        Thread.Sleep(1000);
                        Console.WriteLine($"Close In {i}");
                    }
                    return 0;
                }
                catch (Exception x)
                {
                    Console.WriteLine("Migrate Failed");
                    Console.WriteLine(x.Message);
                    Console.WriteLine(x.Source);
                    Console.WriteLine(x.StackTrace);
                    Thread.Sleep(7000);
                    return -1;
                }
            });
            return flowResult.Item2;
        }
    }
}
