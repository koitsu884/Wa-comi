using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Wacomi.API.Data;
using Wacomi.API.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Wacomi.API.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using NLog.Web;
using Hangfire;

namespace Wacomi.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public CronTask cronTask;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            services.AddCors();
            services.AddMvc();
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.AddAutoMapper();
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("MyDbConnection")));
            //options.UseSqlServer(@"Server=db;Database=WacomiNZ;User=sa;Password=P@ssw0rd!!;"));
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IDataRepository, DataRepository>();

            services.AddIdentity<Account, IdentityRole>
            (o =>
            {
                // configure identity options
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Configure JwtIssuerOptions
            //var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(opeionts =>
                {
                    opeionts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        //ValidateIssuer = false,
                        ValidIssuers = new[] { Configuration.GetSection("JwtIssuerOptions:Issuer").Value },
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddHangfire(config => 
                config.UseSqlServerStorage(Configuration.GetConnectionString("MyDbConnection")));

            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ApplicationDbContext>().Database.Migrate();
            this.cronTask = ActivatorUtilities.CreateInstance<CronTask>(serviceProvider);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var error = context.Features.Get<IExceptionHandlerFeature>();
                        if (error != null)
                        {
                            context.Response.AddApplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }
                    });
                });
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseAuthentication();
            // app.UseHangfireDashboard();
            app.UseHangfireServer();
          //  this.cronTask.StartRssReader();

            if (env.IsDevelopment())
            {
                app.UseMvc();
            }
            else
            {
                app.UseDefaultFiles();
                app.UseStaticFiles();
                app.UseMvc(routes =>
                {
                    routes.MapSpaFallbackRoute(
                        name: "spa-fallback",
                        defaults: new { controller = "Fallback", action = "Index" }
                    );
                });
            }
        }
    }
    
}
