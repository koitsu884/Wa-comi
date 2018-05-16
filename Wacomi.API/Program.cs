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

namespace Wacomi.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                try
                {
                    var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                    var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    Seed.SeedData(userManager, roleManager, context);
                }
                catch (Exception ex)
                {
                    // var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                    // logger.LogError(ex, "An error occurred seeding the DB.");
                }
            }
            host.Run();

            // var host = new WebHostBuilder()  
            // .UseUrls("https://*:5000")
            // .UseKestrel();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                // .UseUrls("http://*:80")
                .Build();
    }
}
