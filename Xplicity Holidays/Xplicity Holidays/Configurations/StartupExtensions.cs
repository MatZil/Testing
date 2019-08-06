using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Xplicity_Holidays.Infrastructure.Database;

namespace Xplicity_Holidays.Configurations
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("holidays", new Info { Title = "Xplicity holidays", Version = "v1" });
            });

            return services;
        }

        public static void ConfigureAndUseSwagger(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                //                c.SwaggerEndpoint("/swagger/v1/swagger.json", "version_demo");
                c.SwaggerEndpoint("/swagger/holidays/swagger.json", "Xplicity");
                c.RoutePrefix = "holidays";
            });
        }

        public static void SetUpDatabase(this IServiceCollection services)
        {
            const string connection = @"Server=localhost\SQLEXPRESS;Database=HolidayDB;Trusted_Connection=True;";
            services.AddDbContext<HolidayDbContext>(options => options.UseSqlServer(connection));
        }

        public static void SetUpAutoMapper(this IServiceCollection services)
        {
            var config = new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperConfiguration());
            });
            var mapper = config.CreateMapper();

            services.AddSingleton(mapper);
        }
    }
}
