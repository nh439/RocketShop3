
using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RocketShop.Database;
using RocketShop.Database.EntityFramework;
using RocketShop.Database.Helper;
using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.Warehouse.Authorization;
using RocketShop.Database.NonEntityFramework;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Helper;
using RocketShop.Migration.Configuration;
using RocketShop.Migration.Model;
using RocketShop.Shared.Helper;

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
                service.InstallDatabase<IdentityContext, AuditLogContext, WarehouseContext,RetailContext>()
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
                    // Inject Database Context
                    var context = scope.ServiceProvider.GetRequiredService<IdentityContext>();
                    var auditContext = scope.ServiceProvider.GetRequiredService<AuditLogContext>();
                    var warehouseContext = scope.ServiceProvider.GetRequiredService<WarehouseContext>();
                    var retailContext = scope.ServiceProvider.GetRequiredService<RetailContext>();
                    // Get Connection String
                    var whConnStr = warehouseContext.Database.GetConnectionString();
                    var clients = configuration.GetSection("Clients").Get<StartClient[]>();
                    var otherConnStr = new[]
                    {
                        auditContext.Database.GetConnectionString(),
                      context.Database.GetConnectionString(),
                        retailContext.Database.GetConnectionString()
                    };
                    if (otherConnStr.Where(x => x == whConnStr).HasData()) /// Check if the warehouse database is shared with other databases
                    {
                        Console.WriteLine("Do not share the warehouse database with other databases.");
                        return -2;
                    }
                    // Migrate
                    context!.Database.Migrate();
                    Console.WriteLine("Identity Migrate Success");
                    auditContext!.Database.Migrate();
                    Console.WriteLine("Audit Log Migrate Success");
                    warehouseContext!.Database.Migrate();
                    Console.WriteLine("Warehouse Migrate Success");
                    retailContext!.Database.Migrate();
                    Console.WriteLine("Retail Migrate Success");
                    Console.WriteLine("End Migrate");
                    // Apply Seed Data
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
                            CreateBy = "SYSTEM"
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
                    #region Client
                    if (clients.HasData())
                    {
                        Console.WriteLine("Creating Clients...");
                        using var transaction = warehouseContext.Database.BeginTransaction();
                        foreach (var client in clients!)
                        {
                            var clientExists = await warehouseContext.Client.AnyAsync(x => x.ClientId == client.ClientId);
                            if (clientExists)
                                continue;
                            var newClient = new Client
                            {
                                Application = client.Application,
                                ClientId = client.ClientId,
                                ClientName = client.ClientName,
                                CreateBy = "SYSTEM",
                                Created = DateTime.UtcNow,
                                RequireSecret = client.ClientSecret.HasMessage(),
                                MaxinumnAccess = 5,
                                TokenExpiration = 3600,
                                Updated = DateTime.UtcNow,
                            };

                            warehouseContext.Client.Add(newClient);
                            await warehouseContext.SaveChangesAsync();
                            List<AllowedObject> allowedObjects = new();
                            if (client.AllowedReadCollections.HasData())
                                allowedObjects.AddRange(client.AllowedReadCollections!.Select(s => new AllowedObject
                                {
                                    Client = newClient.Id,
                                    ObjectName = s
                                }));
                            if (client.AllowedWriteCollections.HasData())
                                allowedObjects.AddRange(client.AllowedWriteCollections!.Select(s => new AllowedObject
                                {
                                    Client = newClient.Id,
                                    ObjectName = s
                                }));
                            allowedObjects = allowedObjects.Distinct().ToList();
                            foreach(AllowedObject allowedObject in allowedObjects)
                            {
                                allowedObject.Read = client.AllowedReadCollections.HasData() && client.AllowedReadCollections!.Where(x => x == allowedObject.ObjectName).HasData();
                                allowedObject.Write = client.AllowedWriteCollections.HasData() && client.AllowedWriteCollections!.Where(x => x == allowedObject.ObjectName).HasData();
                                warehouseContext.AllowedObject.Add(allowedObject);
                            }
                            if(client.ClientSecret.HasMessage())
                            {
                                var secret = client.ClientSecret!.HashPasword(out var salt);
                                warehouseContext.ClientSecret.Add(new ClientSecret
                                {
                                    Client = newClient.Id,
                                    Created = DateTime.UtcNow,
                                    Salt = Convert.ToBase64String(salt),
                                    SecretValue = secret
                                });
                            }
                            await warehouseContext.SaveChangesAsync();
                        }
                       await  transaction.CommitAsync();
                        Console.WriteLine("Create Clients Success");
                    }
                    #endregion
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
                    throw;
                }
            },retries: 3);
            return flowResult.Item2;
        }
    }
}
