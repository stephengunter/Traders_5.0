using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ApplicationCore.DataAccess;
using ApplicationCore.Middlewares;
using Microsoft.EntityFrameworkCore;
using System;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity;
using ApplicationCore.Settings;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApplicationCore;
using ApplicationCore.DI;
using ApplicationCore.Authorization;
using ApplicationCore.Hubs;
using Web.Hubs;
using ApplicationCore.Auth.ApiKey;
using Microsoft.OpenApi.Models;

namespace Web
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            IsDevelopment = env.IsDevelopment();
            Configuration = configuration;
        }

        bool IsDevelopment;

        string ClientUrl => Configuration["AppSettings:ClientUrl"];

        string AdminUrl => Configuration["AppSettings:AdminUrl"];

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            #region Add SqlDatabases
            services.AddDbContext<DefaultContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Default"),
                b => b.MigrationsAssembly("ApplicationCore"))
            );
            services.AddDbContext<RealTimeContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("RealTime"),
                b => b.MigrationsAssembly("ApplicationCore"))
            );
            services.AddDbContext<HistoryContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("History"),
                b => b.MigrationsAssembly("ApplicationCore"))
            );
            #endregion

            #region AddIdentity
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddEntityFrameworkStores<DefaultContext>()
            .AddDefaultTokenProviders();
            #endregion

            #region Add Configurations
            services.Configure<AppSettings>(Configuration.GetSection(SettingsKeys.AppSettings));
            services.Configure<AuthSettings>(Configuration.GetSection(SettingsKeys.AuthSettings));
            services.Configure<CloudStorageSettings>(Configuration.GetSection(SettingsKeys.CloudStorageSettings));
            services.Configure<AdminSettings>(Configuration.GetSection(SettingsKeys.AdminSettings));
            #endregion

            #region  Add ApiKeySupport

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = ApiKeyAuthenticationOptions.DefaultScheme;
                options.DefaultChallengeScheme = ApiKeyAuthenticationOptions.DefaultScheme;
            }).AddApiKeySupport(options => { });

            #endregion

            #region  Add JwtBearer

            string securityKey = Configuration["AuthSettings:SecurityKey"];
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(securityKey));
            string issuer = Configuration["AppSettings:Name"];
            string audience = ClientUrl;
            int tokenValidHours = Convert.ToInt32(Configuration["AuthSettings:TokenValidHours"]);
            services.AddJwtBearer(tokenValidHours, issuer, audience, securityKey);

            #endregion



            if (IsDevelopment)
            {
                services.AddSwagger("Traders Starter", "v1");
            }
           

            services.AddDtoMapper();

            #region Add Authorization Policies

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiKey", policy =>
                {
                    policy.AuthenticationSchemes.Add(ApiKeyAuthenticationOptions.DefaultScheme);
                    policy.Requirements.Add(new HasPermissionRequirement(Permissions.User));
                });

                options.AddPolicy("ApiKey_Admin", policy =>
                {
                    policy.AuthenticationSchemes.Add(ApiKeyAuthenticationOptions.DefaultScheme);
                    policy.Requirements.Add(new HasPermissionRequirement(Permissions.Admin));
                });


                options.AddPolicy(Permissions.Subscriber.ToString(), policy =>
                    policy.Requirements.Add(new HasPermissionRequirement(Permissions.Subscriber)));

                options.AddPolicy(Permissions.Admin.ToString(), policy =>
                    policy.Requirements.Add(new HasPermissionRequirement(Permissions.Admin)));
            });

            #endregion

            #region Add Cors

            services.AddCors(options =>
            {
                options.AddPolicy("Api",
                builder =>
                {
                    builder.WithOrigins(ClientUrl)
                            .AllowAnyHeader()
                            .AllowAnyMethod().AllowCredentials();
                });

                options.AddPolicy("Admin",
                builder =>
                {
                    builder.WithOrigins(AdminUrl)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                });

                
                options.AddPolicy("Global",
                builder =>
                {
                    builder.WithOrigins(ClientUrl, AdminUrl)
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                });
            });

            #endregion


            services.AddControllers();

            services.AddSignalR();


            services.AddHttpClient(HttpClients.Google.ToString(), c =>
            {
                c.BaseAddress = new Uri("https://www.google.com");
            });

            services.AddSingleton<IHubConnectionManager, HubConnectionManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            return AutofacRegister.Register(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //¬O§_¹LÂoIP
            //app.UseMiddleware<IPListMiddleware>(Configuration["IPList"]);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseMiddleware<ExceptionMiddleware>();
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();


            if (env.IsDevelopment())
            {
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Traders");
                });

                app.UseSwagger();
            }
            

            app.UseAuthentication();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<NotificationsHub>("/notifications");
            });
        }
    }
}
