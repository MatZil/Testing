using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Xplicity_Holidays.Configurations;
using Xplicity_Holidays.Infrastructure.Database;
using Xplicity_Holidays.Infrastructure.Database.Models;
using Xplicity_Holidays.Services.Interfaces;

namespace Xplicity_Holidays
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
            services.SetUpIdentity();
            services.SetUpAutoMapper();
            services.SetUpDatabase(Configuration);
            services.SetUpJsonOptions();
            services.AddSwagger();
            services.ConfigureCors();
            services.SetupJtwAuthentication(Configuration);
            services.AddAllDependencies();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IBackgroundService backgroundService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCorsExt();
            app.SetUpStaticFiles(Configuration);
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //use this when running angular together with aspnet core
                    spa.UseAngularCliServer(npmScript: "start");
                    //use this when running them separately
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });

            app.ConfigureAndUseSwagger();
            IdentityDataSeeder.SeedData(userManager, roleManager, Configuration);
            var _ = backgroundService.RunBackgroundServices();
        }
    }
}
