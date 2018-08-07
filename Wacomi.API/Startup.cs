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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc();

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
            services.AddAutoMapper();
            // services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseMySql(Configuration.GetConnectionString("WacomiDbConnection")));
            services.AddSingleton<IEmailSender, EmailSender>();
            services.AddSingleton<IStaticFileManager, StaticFileManager>();
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

            var serviceProvider = services.BuildServiceProvider();
         //   serviceProvider.GetService<ApplicationDbContext>().Database.Migrate();
           // this.cronTask = ActivatorUtilities.CreateInstance<CronTask>(serviceProvider);
            // Add scheduled tasks & scheduler
            services.AddSingleton<IScheduledTask, RssReaderTask>();
            services.AddScheduler((sender, args) =>
            {
                //TODO: log error on database or wherever appropriate
                Console.Write(args.Exception.Message);
                args.SetObserved();
            });
            
            //this.AddLoggingTableAndProcedure(Configuration.GetConnectionString("WacomiDbConnection"));
        }

        private void AddLoggingTableAndProcedure(string connectionString)
        {
            string  createNlogTableCommant = @"
                    IF NOT EXISTS
                    (  SELECT [name] 
                        FROM sys.tables
                        WHERE [name] = 'NLog' 
                    )
                    CREATE TABLE [NLog] (
                    [Id] [int] IDENTITY(1,1) NOT NULL,
                    [Application] [nvarchar](50) NOT NULL,
                    [Logged] [datetime] NOT NULL,
                    [Level] [nvarchar](50) NOT NULL,
                    [Message] [nvarchar](max) NOT NULL,
                    [Logger] [nvarchar](250) NULL,
                    [Callsite] [nvarchar](max) NULL,
                    [Exception] [nvarchar](max) NULL,
                    CONSTRAINT [PK_dbo.Log] PRIMARY KEY CLUSTERED ([Id] ASC)
                    WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
                ) ON [PRIMARY]";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(createNlogTableCommant, connection))
                {
                    command.ExecuteNonQueryAsync().Wait();
                }

                connection.Close();
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IStaticFileManager staticFileManager)
        {
           // loggerFactory.AddNLog();
            // var defaultConnection = Configuration.GetConnectionString("WacomiDbConnection");
            // NLog.GlobalDiagnosticsContext.Set("NLogConnection", defaultConnection);
            staticFileManager.AddStaticFileFolder("feedimages", Configuration.GetSection("BlogFeedImageFolder").Value);
            staticFileManager.AddStaticFileFolder("logs", "static/logs");

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
            
            var feedImageFolder = Path.Combine(Directory.GetCurrentDirectory(), Configuration.GetSection("BlogFeedImageFolder").Value);

            if (env.IsDevelopment())
            {
                //app.UseMvc();
                 app.UseDefaultFiles();
                app.UseStaticFiles();
                foreach(var item in staticFileManager.GetFolderList()){
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        RequestPath = "/" + item.Key,
                        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),item.Value))
                    });
                }

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
                 foreach(var item in staticFileManager.GetFolderList()){
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        RequestPath = "/" + item.Key,
                        FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),item.Value))
                    });
                }

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
