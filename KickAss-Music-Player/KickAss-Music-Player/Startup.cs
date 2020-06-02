using API.Filters;
using KickAss_Music_Player.BusinessLogic.Services.ApplicationVersion;
using KickAss_Music_Player.BusinessLogic.Services.User;
using KickAss_Music_Player.DataModels.Security;
using KickAss_Music_Player.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Autofac;

namespace KickAss_Music_Player
{
    public class Startup
    {
        private KeycloakTokenValidationParameters _keycloakTokenValidationParameter;

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            //Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }

        private static IContainer Container { get; set; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            var authenticationSettings = Configuration.GetSection("AuthenticationSettings").Get<AuthenticationSetting>();
            _keycloakTokenValidationParameter = new KeycloakTokenValidationParameters(authenticationSettings);
            services.AddSingleton(_keycloakTokenValidationParameter);
            services.AddCors();
            
            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-TOKEN");

            // Autofac
            #region
            var autofacServiceProvider = new ContainerBuilder();

            autofacServiceProvider.RegisterType<ApplicationVersionService>().As<IApplicationVersionService>();
            autofacServiceProvider.RegisterType<UserService>().As<IUserService>();
            autofacServiceProvider.RegisterType<TokenInterpretor>().As<ITokenInterpretor>();

            Container = autofacServiceProvider.Build();

            services.AddSingleton<IContainer>(Container);
            #endregion

            var userSvc = Container.Resolve<IUserService>();
            services.AddMvc(config =>
            {
                config.Filters.Add(new CreatedModifiedFilterAttribute(new TokenInterpretor(userSvc)));
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddOpenIdConnect(options =>
            {
                var authenticationSetting = Configuration.GetSection("AuthenticationSettings").Get<AuthenticationSetting>();
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Cookies
                options.RequireHttpsMetadata = authenticationSetting.RequireHttps ?? true; // For dev purposes only
                options.Authority = authenticationSetting.AuthorityHost;
                options.ClientId = authenticationSetting.AuthorityClientId;
                options.ClientSecret = authenticationSetting.AuthorityClientSecret;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.GetClaimsFromUserInfoEndpoint = true;
                options.SaveTokens = true;
                options.UseTokenLifetime = true;
                options.TokenValidationParameters = _keycloakTokenValidationParameter;
                options.SecurityTokenValidator = new JwtSecurityTokenHandler();
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = _keycloakTokenValidationParameter;
                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = TokenAuthFailed,
                };
                options.RequireHttpsMetadata = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(builder => builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .SetPreflightMaxAge(new TimeSpan(0, 1, 0, 0))
                            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //app.UseMvc(routes =>
            //{
            //    routes.MapRoute("default", "api/{controller}/{action=Index}/{id?}");
            //});
        }

        private Task TokenAuthFailed(AuthenticationFailedContext arg)
        {
            // You can validate here when the Token is not valid
            // see arg.Exception
            return Task.CompletedTask;
        }
    }
}
