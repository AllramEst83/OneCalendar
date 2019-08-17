using System.Text;
using AutoMapper;
using TokenValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OneCalendar.Context;
using OneCalendar.Data;
using OneCalendar.Helpers.Settings;
using OneCalendar.Interfaces;
using OneCalendar.Models;
using OneCalendar.Services;

namespace OneCalendar
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

            string section = "AppSettings";
            string assemblyName = "OneCalendar";
            IConfigurationSection configuration = Configuration.GetSection(section);
            services.Configure<AppSettings>(configuration);
            AppSettings appsettings = configuration.Get<AppSettings>();

            services.AddDbContext<UserContext>(options =>
            {
                options.UseSqlServer(appsettings.AuthDbConnectionString,
                    migrationOptions =>
                    {
                        migrationOptions.MigrationsAssembly(assemblyName);
                    });
            });

            services.AddDbContext<CalenderContext>(optionsCal =>
            {
                optionsCal.UseSqlServer(appsettings.CalenderConnectionString,
                    migrationOptions =>
                    {
                        migrationOptions.MigrationsAssembly(assemblyName);
                    });
            });

            SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appsettings.Secret));
            SigningCredentials signingCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);

            services.Configure<JwtIssuerOptions>(options =>
            {
                options.Issuer = appsettings.Issuer;
                options.Audience = appsettings.Audience;
                options.SigningCredentials = signingCredentials;
            });

            IdentityBuilder builder = services.AddIdentityCore<User>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder
                .AddEntityFrameworkStores<UserContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>();

            services.AddValidationParameters(
                       appsettings.Issuer,
                       appsettings.Audience,
                       _signingKey
                       );

            services.AddAutoMapper();

            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ISeedService, SeedService>();
            services.AddScoped<ICalenderService, CalenderService>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                       .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
                       .AddJsonOptions(options =>
                       {
                           options.SerializerSettings.Formatting = Formatting.Indented;
                           options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                       });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ISeedService seedService, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //seedService.SeedCalenderTasks();

            app.UseHttpsRedirection();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin() // TODO: revisit and check if this can be more strict and still allow preflight OPTION requests
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}


//To attach web app tp process:
//https://www.locktar.nl/uncategorized/azure-remote-debugging-manually-in-visual-studio-2017/