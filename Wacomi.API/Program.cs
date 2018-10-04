using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Wacomi.API.Data;
using Wacomi.API.Models;
using NLog.Web;
using System.Net.Http;
using Wacomi.API.Helper;

namespace Wacomi.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                // logger.Info("init main");
                // logger.Error("error log test");
                var host = BuildWebHost(args);
                // using (var scope = host.Services.CreateScope())
                // {
                //     var serviceProvider = scope.ServiceProvider;

                //     var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                //     var userManager = serviceProvider.GetRequiredService<UserManager<Account>>();
                //     var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                //     var seeder = serviceProvider.GetService<IDatabaseSeeder>();
                //     seeder.Seed(userManager, roleManager, context);

                //     //Seed.SeedData(userManager, roleManager, context);

                // }
                // RecurringJob.AddOrUpdate(
                //     () => Test(),
                //     Cron.Minutely);
                host.Run();


            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw ex;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }

            // var host = new WebHostBuilder()  
            // .UseUrls("https://*:5000")
            // .UseKestrel();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                // .ConfigureLogging(( hostingContext, logging) => {
                //     logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                //     logging.AddConsole();
                //     logging.AddDebug();
                // })
                // .UseUrls("http://*:80") //Docker
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
                })
                .UseNLog()
                // .UseSetting("detailedErrors", "true")
                // .CaptureStartupErrors(true)
                .Build();
    }
}
