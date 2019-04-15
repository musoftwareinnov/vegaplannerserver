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
using Scheduler.Code;
using Scheduler.Code.Scheduling;
using vegaplannerserver.Core;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using vegaplannerserver.Core.Security;
using vega.Services.Interfaces;
using vega.Services;
using System.Collections.Generic;
using vega.Core.Models.States;
using vega.Services.Implementations;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;

namespace vega
{
    public class DateSettings
    {
         public string CurrentDateOverride { get; set; }
    }
    public class Startup
    {
        //Secret Key for JWT generation MOVE TO AZURE SECRET KEYS
        private const string SecretKey = "H14LwZOakkopDONR3tiEGqcsz6tgLjNl"; // gen from https://randomkeygen.com/
        private readonly SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

        
        //setup CORS access (TODO : move to settings file)
        private string[] allowCors = {
            "http://localhost:4200",
            "https://vegaplannerclient.azurewebsites.net",
            "https://vegaplannerclientcds.azurewebsites.net"
        };


        public Startup(IHostingEnvironment env, IConfiguration azureConfiguration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            //Add azure secret configuration that contain local keyvault values
            builder.AddConfiguration(azureConfiguration);    
            Configuration = builder.Build(); 
        }


        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Core Repositories
            services.AddScoped<IDateService, DateService>();
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
            services.AddScoped<IDescriptionOfWorkRepository, DescriptionOfWorkRepository>(); 
            services.AddScoped<IDrawingRepository, DrawingRepository>(); 
            services.AddScoped<IPlanningStatisticsRepository, PlanningStatisticsRepository>(); 
            services.AddScoped<IBusinessDateRepository, BusinessDateRepository>(); 
            services.AddScoped<IFeeRepository, FeeRepository>(); 
            services.AddScoped<IStaticDataRepository, StaticDataRepository>(); 
            services.AddScoped<IProjectGeneratorRepository, ProjectGeneratorRepository>(); 

            //Core Services
            services.AddScoped<IPlanningAppService, PlanningAppService>(); 
            services.AddScoped<IPlanningAppStateService, PlanningAppStateService>(); 
         
            //Security 
            services.AddScoped<UserManager<AppUser>>();
            services.AddScoped<RoleManager<IdentityRole>>();
            services.AddScoped<IUserRepository, UserRepository>(); 
            services.AddSingleton<IJwtFactory, JwtFactory>();
            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();

            // Add scheduled tasks & scheduler NOT USED ATM
            //services.AddSingleton<IScheduledTask, QuoteOfTheDayTask>();
            //services.AddSingleton<IScheduledTask, RollBusinessDate>();
            // services.AddScheduler((sender, args) =>
            // {
            //     Console.Write(args.Exception.Message);
            //     args.SetObserved();
            // });

            //Allow Cross Origin
            services.AddCors(options => options.AddPolicy("AllowAll", 
                o => o.WithOrigins(allowCors).AllowAnyMethod().AllowAnyHeader()
                ));


            //Unit Of Work Design Pattern
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //AutoMapper
            services.AddAutoMapper(typeof(Startup));

            //Database Connection. Get Password From KeyVault
            var connectionString =  Configuration.GetConnectionString("Default") + Configuration["VegaPlannerDbPwdCDS"];
            Console.WriteLine(connectionString); //REMOVE WHEN HAPPY!!
            services.AddDbContext<VegaDbContext>(options => options.UseSqlServer(connectionString));
            //services.AddDbContext<VegaDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Default") ));
  
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
                options.AddPolicy("ApiUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.rol, 
                                                        Constants.Strings.JwtClaims.AdminUser, 
                                                        Constants.Strings.JwtClaims.DesignerSurveyUser,
                                                        Constants.Strings.JwtClaims.DesignerDrawingUser));

                options.AddPolicy("AdminUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.rol, 
                                                        Constants.Strings.JwtClaims.AdminUser));

                //Not Currently used                                            
                options.AddPolicy("CustomerUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.rol, Constants.Strings.JwtClaims.AdminUser,Constants.Strings.JwtClaims.CustomerMaintenenceUser));
                options.AddPolicy("NextStateUser", policy => policy.RequireClaim(Constants.Strings.JwtClaimIdentifiers.rol, Constants.Strings.JwtClaims.AdminUser,Constants.Strings.JwtClaims.NextStateUser));
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

            //Set identity to connect to db contect
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);           
            builder.AddEntityFrameworkStores<VegaDbContext>().AddDefaultTokenProviders();

            //MVC
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,          
                              IHostingEnvironment env, 
                              RoleManager<IdentityRole> roleManager,
                              IBusinessDateRepository businessDateRepository, 
                              IDateService dateService,
                              IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                Console.WriteLine("In Development mode");
                Console.WriteLine("Database Connection String: " + Configuration.GetConnectionString("Default"));              
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
            app.UseAuthentication();

            //Create Roles if not exist
            //CORE DUMP ONCE SET!!!!!!!! TODO!!!!!!
            CreateUserRoles(serviceProvider).Wait();

            //Create Admin User if not exist
            CreateAdminUser(serviceProvider).Wait();

            //Create List Of Status if not already set
            CreatePlanningAppStatuses(serviceProvider).Wait();

            //Set global date, ovverride if set
            var options = new DateSettings();
            Configuration.GetSection("DateSettings").Bind(options);
            setApplicationDate(options.CurrentDateOverride, dateService, businessDateRepository);
        }

        // This method fetches a token from Azure Active Directory, which can then be provided to Azure Key Vault to authenticate
        public async Task<string> GetAccessTokenAsync()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            string accessToken = await azureServiceTokenProvider.GetAccessTokenAsync("https://vault.azure.net");
            return accessToken;
        }
        private async Task CreatePlanningAppStatuses(IServiceProvider serviceProvider)  
        { 
            var UnitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();  
            var StateStatusRepository = serviceProvider.GetRequiredService<IStateStatusRepository>();  

            var ctr = await StateStatusRepository.GetStateStatusList();
            if(ctr.Count == 0) {
                var stateStatus = new StateStatus { GroupType = "InProgress", Name = "InProgress", OrderId = 1 };
                StateStatusRepository.AddStateStatusList(stateStatus);
                stateStatus = new StateStatus { GroupType = "InProgress", Name = "Overdue", OrderId = 2 };
                StateStatusRepository.AddStateStatusList(stateStatus);
                stateStatus = new StateStatus { GroupType = "InProgress", Name = "Due", OrderId = 3 };
                StateStatusRepository.AddStateStatusList(stateStatus);
                stateStatus = new StateStatus { GroupType = "InProgress", Name = "OnTime", OrderId = 4 };
                StateStatusRepository.AddStateStatusList(stateStatus);
                stateStatus = new StateStatus { GroupType = "Not InProgress", Name = "Not InProgress", OrderId = 5 };
                StateStatusRepository.AddStateStatusList(stateStatus);
                stateStatus = new StateStatus { GroupType = "Not InProgress", Name = "Complete", OrderId = 6 };
                StateStatusRepository.AddStateStatusList(stateStatus);
                stateStatus = new StateStatus { GroupType = "Not InProgress", Name = "Overran", OrderId = 7 };
                StateStatusRepository.AddStateStatusList(stateStatus);
                stateStatus = new StateStatus { GroupType = "Not InProgress", Name = "Terminated", OrderId = 8 };
                StateStatusRepository.AddStateStatusList(stateStatus);
                stateStatus = new StateStatus { GroupType = "Not InProgress", Name = "Archived", OrderId = 9 };
                StateStatusRepository.AddStateStatusList(stateStatus);
                stateStatus = new StateStatus { GroupType = "All", Name = "All", OrderId = 10 };
                StateStatusRepository.AddStateStatusList(stateStatus);

            }
            await UnitOfWork.CompleteAsync();
        } 

        private async Task CreateAdminUser(IServiceProvider serviceProvider)  
        {     
            var UserManager = serviceProvider.GetRequiredService<UserManager<AppUser>>(); 
            var UserRepository = serviceProvider.GetRequiredService<IUserRepository>();
            var UnitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();  
            // Add admin user if not already exist
            var user = await UserManager.FindByEmailAsync(Constants.Strings.AdminUser.Email); 

            if(user == null) {
                var userIdentity = new AppUser();

                userIdentity.UserName = Constants.Strings.AdminUser.Email;
                userIdentity.FirstName = Constants.Strings.AdminUser.FirstName;
                userIdentity.LastName = Constants.Strings.AdminUser.LastName;
                userIdentity.Email = Constants.Strings.AdminUser.Email;
        
                var result = await UserManager.CreateAsync(userIdentity, "admpassword"); //CHANGE To ENCRYPT!!!!!!

                if (result.Succeeded) {
                    //IAU Deprecated -> UserRepository.Add(new InternalAppUser { IdentityId = userIdentity.Id, Location = "Nowhere" });    
                    var adminClaims = RoleClaimSetup.RoleClaimSetupAdmin();
                    await UserManager.AddToRoleAsync(userIdentity, adminClaims.Role.Name); 
                    await UnitOfWork.CompleteAsync();
                }
            } 
        }
        private async Task CreateUserRoles(IServiceProvider serviceProvider)  
        {     
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();       

            bool x = await RoleManager.RoleExistsAsync(Constants.Strings.JwtClaims.AdminUser);
            if (!x)
            {

                var claims = RoleClaimSetup.RoleClaimSetupAdmin();
                await RoleManager.CreateAsync(claims.Role);

                foreach(var claim in claims.Claims) {
                    await RoleManager.AddClaimAsync(claims.Role, claim);
                }
            }

            //Role : Designer Survey
            x = await RoleManager.RoleExistsAsync(Constants.Strings.JwtClaims.DesignerSurveyUser);
            if (!x) {
                 var claims = RoleClaimSetup.RoleClaimSetupDesignerSurvey();
                await RoleManager.CreateAsync(claims.Role);

                foreach(var claim in claims.Claims) {
                    await RoleManager.AddClaimAsync(claims.Role, claim);  
                }             
            }

            x = await RoleManager.RoleExistsAsync(Constants.Strings.JwtClaims.DesignerDrawingUser);
            if (!x) {
                 var claims = RoleClaimSetup.RoleClaimSetupDesignerDrawer();
                await RoleManager.CreateAsync(claims.Role);

                foreach(var claim in claims.Claims) {
                    await RoleManager.AddClaimAsync(claims.Role, claim);  
                }             
            }           
        }

        private async void setupClaimsForRole(RoleManager<IdentityRole> roleManager, RoleClaim roleClaims)
        {
                await roleManager.CreateAsync(roleClaims.Role);

                foreach(var claim in roleClaims.Claims) {
                    await roleManager.AddClaimAsync(roleClaims.Role, claim);
                } 
        }
        
        public void setApplicationDate(string currentDateOverride, IDateService dateService, IBusinessDateRepository businessDateRepository)
        {
            var businessDate = !string.IsNullOrEmpty(currentDateOverride) ? currentDateOverride.ParseInputDate() : DateTime.Now.Date;

            dateService.SetCurrentDate(businessDate);          

        }
    }
    
}
