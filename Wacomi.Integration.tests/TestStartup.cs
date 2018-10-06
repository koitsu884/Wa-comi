using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Wacomi.API;
using Wacomi.API.Data;
using Wacomi.API.Helper;
using Wacomi.Integration.tests.Helper;

namespace Wacomi.Integration.tests
{
    public class TestStartup : Startup
    {
        public TestStartup(IConfiguration configuration, IHostingEnvironment env) : base(configuration, env)
        {
        }

        public override void ConfigureDatabase(IServiceCollection services)
        {
        //     services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseMySql(Configuration.GetConnectionString("WacomiTestDbConnection")));
        //     var serviceProvider = services.BuildServiceProvider();
        //    // serviceProvider.GetService<ApplicationDbContext>().Database.Migrate();
        //     services.AddScoped<IDatabaseSeeder, TestDataSeeder>();

            services.AddDbContext<ApplicationDbContext>(options =>
            //    options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
                options.UseInMemoryDatabase("WacomiNZ_Test"));

            services.AddScoped<IDatabaseSeeder, TestDataSeeder>();
        }
    }
}