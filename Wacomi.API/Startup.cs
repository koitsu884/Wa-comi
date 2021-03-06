﻿using System;
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
// using NLog.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Routing;
using System.IO;
using Microsoft.Extensions.FileProviders;
using System.Data.SqlClient;
using NLog.Extensions.Logging;
using Wacomi.API.Scheduling;
using Wacomi.API.Scheduling.CronTasks;

namespace Wacomi.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IHostingEnvironment CurrentEnvironment { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
        }

        public virtual void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("WacomiDbConnection")));
            var serviceProvider = services.BuildServiceProvider();
            serviceProvider.GetService<ApplicationDbContext>().Database.Migrate();
            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc()
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDataProtection();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.Configure<AuthMessageSenderOptions>(Configuration);
            // if (CurrentEnvironment.IsProduction())
            // {
            //     services.Configure<MvcOptions>(options =>
            //     {
            //         options.Filters.Add(new RequireHttpsAttribute());
            //     });
            // }

            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.Configure<AuthMessageSenderOptions>(Configuration.GetSection("MessageSenderOptions"));
            //services.Configure<AuthMessageSenderOptions>(Configuration.GetSection("MessageSenderOptionsSG"));
            //services.Configure<AuthMessageSenderOptions>(Configuration.GetSection("MessageSenderOptionsSB"));
            services.AddAutoMapper();
            // services.AddLogging();
            ConfigureDatabase(services);
            services.AddSingleton<IEmailSender, EmailSender>();
            //services.AddSingleton<IEmailSender, SendGlidManager>();
            //services.AddSingleton<IEmailSender, MailGunManager>();
            services.AddSingleton<ImageFileStorageManager>();

            //=============== Repository registoration =================
            services.AddScoped<IDataRepository, DataRepository>();
            services.AddScoped<IAdminDataRepository, AdminDataRepository>();
            services.AddScoped<IAppUserRepository, AppUserRepository>();
            services.AddScoped<IAttractionRepository, AttractionRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();
            services.AddScoped<IBlogRepository, BlogRepository>();
            services.AddScoped<IClanSeekRepository, ClanSeekRepository>();
            services.AddScoped<IDailyTopicRepository, DailyTopicRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IPropertySeekRepository, PropertySeekRepository>();
            services.AddScoped<ICircleRepository, CircleRepository>();

            services.AddIdentity<Account, IdentityRole>
            (o =>
            {
                // configure identity options
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
                o.Lockout.MaxFailedAccessAttempts = 20;
                o.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(24);
                o.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

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

            // Add scheduled tasks & scheduler
            services.AddSingleton<IScheduledTask, RssReaderTask>();
            services.AddSingleton<IScheduledTask, DeleteOldFeedTask>();
            services.AddSingleton<IScheduledTask, ChangeTopicTask>();
            services.AddScheduler((sender, args) =>
            {
                //TODO: log error on database or wherever appropriate
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                // var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
                // var userManager = serviceProvider.GetRequiredService<UserManager<Account>>();
                // var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var seeder = serviceProvider.GetService<IDatabaseSeeder>();
                seeder.Seed();

                //Seed.SeedData(userManager, roleManager, context);

            }

            //staticFileManager.AddStaticFileFolder("static", "static");

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

            //var feedImageFolder = Path.Combine(Directory.GetCurrentDirectory(), Configuration.GetSection("BlogFeedImageFolder").Value);

            if (env.IsDevelopment())
            {
                //app.UseMvc();
                app.UseDefaultFiles();
                app.UseStaticFiles();
                app.UseStaticFiles(new StaticFileOptions
                {
                    RequestPath = "/static",
                    FileProvider = new PhysicalFileProvider(@"C:/Angular_Core/Wacomi/Wacomi.API/static")
                });
                // foreach(var item in staticFileManager.GetFolderList()){
                //     app.UseStaticFiles(new StaticFileOptions
                //     {
                //         RequestPath = "/" + item.Key,
                //         FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),item.Value))
                //     });
                // }

                app.UseMvc(routes =>
                {
                    routes.MapSpaFallbackRoute(
                        name: "spa-fallback",
                        defaults: new { controller = "Fallback", action = "Index" }
                    );
                });
            }
            else
            {
                // var options = new RewriteOptions()
                // .AddRedirectToHttpsPermanent();
                // app.UseRewriter(options);

                app.UseLetsEncryptFolder(env);
                app.UseDefaultFiles();
                app.UseStaticFiles();
                app.UseStaticFiles(new StaticFileOptions
                {
                    RequestPath = "/static",
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "static"))
                });
                //  foreach(var item in staticFileManager.GetFolderList()){
                //     app.UseStaticFiles(new StaticFileOptions
                //     {
                //         RequestPath = "/" + item.Key,
                //         FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),item.Value))
                //     });
                //}

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
