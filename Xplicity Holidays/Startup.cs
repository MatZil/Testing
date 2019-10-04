using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xplicity_Holidays.Configurations;
using Xplicity_Holidays.Infrastructure.Database;
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
            services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<HolidayDbContext>();
            services.SetUpAutoMapper();
            services.SetUpDatabase(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwagger();
            services.AddCors();
            services.SetupJtwAuthentication(Configuration);
            services.AddAllDependencies();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IBackgroundService backgroundService)
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
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
            app.ConfigureAndUseSwagger();

            var _ = backgroundService.RunBackgroundServices();
        }
    }
}
