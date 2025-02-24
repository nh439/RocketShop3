﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RocketShop.Framework.Extension;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using LanguageExt.Common;

namespace RocketShop.Framework.Helper
{
    public static class StartupHelper
    {
        public static IConfiguration InstallConfiguration(this IHostApplicationBuilder builder) 
        {
                IConfiguration conf = (new ConfigurationBuilder())
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json",false,true)
                .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json",true,true)
                .AddEnvironmentVariables()
                .Build();
            return conf;
        }
        public static IServiceCollection InstallServices(this IServiceCollection services,
            Action<IServiceCollection> ServiceToInstall)
        {
            ServiceToInstall(services);
            return services;
        }
        public static IHostApplicationBuilder InstallSerilog(this IHostApplicationBuilder builder,
            string logPath = "../log/serilog_Service.log",
            string template = "- {Timestamp:u} {SourceContext} [{Level}]{NewLine} {Message}{NewLine}{Exception}---END---{NewLine}")
        {
            var logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate:template)
    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day,outputTemplate:template)
    .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
            builder.Services.AddSingleton(Log.Logger);
            return builder;
        }

        public static IHostApplicationBuilder InstallServices(this IHostApplicationBuilder builder,
             Action<IServiceCollection> ServiceToInstall)
        {
            builder.Services.InstallServices(ServiceToInstall);
            return builder;
        }

        public static IServiceProvider InvokeEntryFlow(this IServiceProvider provider,Action<IServiceScope> action, int? retries = null, int intervalSecond = 3)
        {
            while (true)
            {
                try
                {
                    using var scope = provider.CreateScope();
                    action(scope);
                    return provider;
                }
                catch
                {
                    if (retries.HasValue)
                    {
                        retries -= 1;
                        Console.WriteLine("Something went wrong, retrying...");
                        if (retries == 0)
                        {
                            Console.WriteLine("Retry limit reached");
                            throw;
                        }
                    }
                    Thread.Sleep(intervalSecond * 1000);
                }
            }
        }
         public static (IServiceProvider,int) InvokeEntryFlowAndReturn(this IServiceProvider provider,Func<IServiceScope,int> action, int? retries = null, int intervalSecond = 3)
        {
            while (true)
            {
                try
                {
                    using var scope = provider.CreateScope();
                    var result = action.Invoke(scope);
                    return (provider, result);
                }
                catch
                {
                    if (retries.HasValue)
                    {
                        retries -= 1;
                        Console.WriteLine("Something went wrong, retrying...");
                        if (retries == 0)
                        {
                            Console.WriteLine("Retry limit reached");
                            throw;
                        }
                    }
                    Thread.Sleep(intervalSecond * 1000);
                }

            }
        }
         public static async Task< (IServiceProvider,int)> InvokeEntryFlowAndReturn(this IServiceProvider provider, Func<IServiceScope,Task< int>> action,int? retries = null,int intervalSecond = 3)
        {
            while (true)
            {
                try
                {
                    using var scope = provider.CreateScope();
                    var result = await action.Invoke(scope);
                    return (provider, result);
                }
                catch
                {
                    if (retries.HasValue)
                    {
                        retries -= 1;
                        Console.WriteLine("Something went wrong, retrying...");
                        if (retries == 0)
                        {
                            Console.WriteLine("Retry limit reached");
                            throw;
                        }
                    }
                    await Task.Delay(intervalSecond * 1000);
                }
                
            }
        }

    }
}
