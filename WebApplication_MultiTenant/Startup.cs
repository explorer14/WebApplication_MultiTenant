using ApplicationServices;
using DataAccess;
using DataAcess;
using DataAcess.DomainModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using WebApplication_MultiTenant.AuthHelpers;
using WebApplication_MultiTenant.CustomMiddleware;

namespace WebApplication_MultiTenant
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IDbContextFactory, DbContextFactory>(serviceProvider => new DbContextFactory(Configuration["ConnectionStringTemplate"]));

            services.AddSingleton<IIdentityResolver, InMemoryIdentityResolver>();
            services.AddSingleton<IJwtTokenGenerator, JWTTokenGenerator>();

            SecurityKey signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Token:SigningKey"]));

            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                ValidateIssuer = true,
                ValidIssuer = Configuration["Token:Issuer"],

                ValidateAudience = true,
                ValidAudience = Configuration["Token:Audience"],

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                RequireExpirationTime = true
            };

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie(options =>
                    {
                        options.TicketDataFormat = new CustomJwtDataFormat(SecurityAlgorithms.HmacSha256, tokenValidationParameters);
                        options.Cookie.Expiration = TimeSpan.FromMinutes(5);
                        options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
                        options.LoginPath = "/Auth/Login";
                        options.LogoutPath = "/Auth/Logout";
                        options.ReturnUrlParameter = "returnUrl";
                        options.AccessDeniedPath = options.LoginPath;
                    });

            // AddCookie if you want to use simple cookie based auth without relying on ASP.NET
            // Identity. AddCookie with a custom ticket data format if you are sending JWT in a
            // cookie so that subsequent authentication browser requests automatically send this info
            // back to the server (Current case). AddJwtBearer if you are using JWT token auth and
            // relying on JS clients to send the token back in the Authorisation header with every
            // request. This will only work from AJAX calls, not for full form post backs as I have
            // yet to find a way to inject custom headers in a full form post back
            services.AddMvc(options => options.Filters.Add(new RequireHttpsAttribute()));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseTenantDBMapper();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{tenant}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "login",
                    template: "{controller=Auth}/{action=Login}");
            });
        }
    }
}