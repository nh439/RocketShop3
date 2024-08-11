
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Helper;
using RocketShop.Database.Model.Identity;
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
                service.InstallDatabase<IdentityContext>()
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
                    context!.Database.Migrate();
                    Console.WriteLine("End Migrate");
                    Console.WriteLine("Create First User");
                    using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var exists = await userManager.FindByEmailAsync(startUser!.Email);
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
                            CreateBy="SYSTEM"
                        };
                        await userManager.CreateAsync(user, startUser.Password);
                        context.UserRole.Add(new UserRole
                        {
                            RoleId = 1,
                            UserId = user.Id
                        });
                        context.UserInformation.Add(new UserInformation
                        {
                            BrithDay = DateTime.UtcNow,
                            StartWorkDate = DateTime.UtcNow,
                            UserId = user.Id
                        });
                        await context.SaveChangesAsync();
                        Console.WriteLine("First User Create Successful");
                    }
                    else
                        Console.WriteLine("First User Already Exists Skipped");
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
