using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XplicityApp.Configurations;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using XplicityApp.Infrastructure.Utils;

namespace XplicityApp
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
            services.SetUpAudit();
            services.AddHealthChecks()
                    .AddCheck<HealthCheck>("HealthCheck");
            services.AddRazorPages();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            AzureStorageConfiguration.Configure(app.ApplicationServices.GetRequiredService<IConfiguration>());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCorsExt();
            app.UseHttpsRedirection();

            app.SetUpStaticFiles(Configuration);
            app.SetUpAzureStorage();
            //app.AddCorsRuleForAzure();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.ConfigureAndUseSwagger();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                { 
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200/"); 
                 // spa.UseAngularCliServer(npmScript: "start");
                }
            });

            IdentityDataSeeder.SeedData(userManager, roleManager, Configuration);
        }
    }
}
