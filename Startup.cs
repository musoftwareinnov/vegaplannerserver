using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Persistence;
using AutoMapper;
using vega.Core.Models;
using vega.Core.Models.Settings;
using System;
using vega.Extensions.DateTime;
using Microsoft.Extensions.Options;
using vegaplanner.Core.Models.Security.Helpers;
using vega.Core.Utils;
using vegaplanner.Core.Models.Security;
using vegaplanner.Core.Models.Security.Persistence;
using Microsoft.AspNetCore.Identity;
using vegaplanner.Core.Models.Security.JWT;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FluentValidation.AspNetCore;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using vegaplanner.Core.Models.Security.Auth;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Http;

namespace vega
{
    public class DateSettings
    {
         public string CurrentDateOverride { get; set; }
    }
    public class Startup
    {
        //Secret Key for JWT generation
        private const string SecretKey = "H14LwZOakkopDONR3tiEGqcsz6tgLjNl"; // gen from https://randomkeygen.com/
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        //setup CORS access (TODO : move to settings file)
        private string[] allowCors = {
            "http://localhost:4200",
            "https://vegaplannerclient.azurewebsites.net"
        };

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            //Set global date, ovverride if set
            var options = new DateSettings();
            Configuration.GetSection("DateSettings").Bind(options);
            setApplicationDate(options.CurrentDateOverride);
        }

        public void setApplicationDate(string currentDateOverride)
        {
            var currentDate = DateTime.Now;
            if(currentDateOverride != "")
                currentDate = currentDateOverride.ParseInputDate();

            SystemDate.Instance.date = currentDate;
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Core Repositories
            services.Configure<PhotoSettings>(Configuration.GetSection("PhotoSettings"));
            services.Configure<StateStatusSettings>(Configuration.GetSection("StateStatusSettings"));
            services.Configure<DateFormatSetting>(Configuration.GetSection("DateFormatSetting"));
            services.Configure<DateSettings>(Configuration.GetSection("DateSettings"));
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IStateInitialiserStateRepository, StateInitialiserStateRepository>();
            services.AddScoped<IStateInitialiserRepository, StateInitialiserRepository>();
            services.AddScoped<IPlanningAppRepository, PlanningAppRepository>();
            services.AddScoped<IPlanningAppStateRepository, PlanningAppStateRepository>();
            services.AddScoped<IStateStatusRepository, StateStatusRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IDrawingRepository, DrawingRepository>(); 
            services.AddScoped<IPlanningStatisticsRepository, PlanningStatisticsRepository>(); 

            //Security 
            services.AddScoped<IUserRepository, UserRepository>(); 
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

            //Allow Cross Origin
            services.AddCors(options => options.AddPolicy("AllowAll", 
                o => o.WithOrigins(allowCors).AllowAnyMethod().AllowAnyHeader()
                ));


            //Unit Of Work Design Pattern
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //AutoMapper
            services.AddAutoMapper();

            //Database Connection
            services.AddDbContext<VegaDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default")));
  
            // SECURITY && JWT Wire up
            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
            });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(configureOptions =>
                {
                    configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                    configureOptions.TokenValidationParameters = tokenValidationParameters;
                    configureOptions.SaveToken = true;
            });

            // api user claim policy
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.Rol, Constants.Strings.JwtClaims.ApiAccess));
            });

            // SECURITY && JWT Setup END

            // Configure identity options
            var builder = services.AddIdentityCore<AppUser>(o =>
            {

                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<VegaDbContext>().AddDefaultTokenProviders();

            //MVC
            services.AddMvc().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
                // app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                // {
                //     HotModuleReplacement = true
                // });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseExceptionHandler(
                builder =>
                {
                    builder.Run(
                            async context =>
                            {
                                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                                var error = context.Features.Get<IExceptionHandlerFeature>();
                                if (error != null)
                                {
                                    
                                    //context.Response.AddApplicationError(error.Error.Message);
                                    await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                                }
                        });
                });

            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseCors("AllowAll");
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {   
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });

            
        }
    }
}
