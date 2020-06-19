using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using XplicityApp.Infrastructure.Database;
using XplicityApp.Infrastructure.Database.Models;
using Audit.Core;
using Azure.Storage.Blobs;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Shared.Protocol;
using System.Collections.Generic;

namespace XplicityApp.Configurations
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("holidays", new OpenApiInfo { Title = "Xplicity holidays", Version = "v1" });
            });

            return services;
        }

        public static void SetUpJsonOptions(this IServiceCollection services)
        {
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Formatting = Formatting.Indented;
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }

        public static void ConfigureAndUseSwagger(this IApplicationBuilder app)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/holidays/swagger.json", "Xplicity");
                options.RoutePrefix = "swagger";
            });
        }

        public static void SetUpIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<HolidayDbContext>();
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredUniqueChars = 0;
            });
        }

        public static void SetUpDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                services.AddDbContext<HolidayDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("MyDbConnection")));

                services.BuildServiceProvider().GetService<HolidayDbContext>().Database.Migrate();
            }
            else
            {
                var connectionString = configuration["Database:ConnectionString"];
                services.AddDbContext<HolidayDbContext>(options => options.UseSqlServer(connectionString));
            }
        }

        public static void SetUpAudit(this IServiceCollection services)
        {
            Audit.Core.Configuration.Setup()
                .UseEntityFramework(ef => ef
                    .AuditTypeMapper(type => typeof(AuditLog))
                        .AuditEntityAction<AuditLog>((auditEvent, auditEntry, auditObject) =>
                        {
                            auditObject.Data = auditEntry.ToJson();
                            auditObject.EntityType = auditEntry.EntityType.Name;
                            auditObject.Date = DateTime.Now;
                            auditObject.User = Environment.UserName;
                        })
                .IgnoreMatchedProperties(true));
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

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("ExposeResponseHeaders", policyBuilder =>
            {
                policyBuilder.WithExposedHeaders("Content-Disposition");
            }));
        }

        public static void UseCorsExt(this IApplicationBuilder app)
        {
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                //.AllowCredentials()
                .WithExposedHeaders("Content-Disposition")
            );
        }

        public static void SetUpStaticFiles(this IApplicationBuilder app, IConfiguration configuration)
        {
            var baseFolder = configuration.GetValue<string>("FileConfig:BaseFolder");
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), baseFolder)),
                RequestPath = new PathString(string.Concat("/", baseFolder))
            });
        }
        public static async void SetUpAzureStorage(this IApplicationBuilder app)
        {
            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);

            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            string[] directories = { "documents", "images", "orders", "policy", "requests", "stylesheets", "unknown" };

            foreach (var dir in directories)
            {
                CloudBlobContainer container = blobClient.GetContainerReference(dir);
                if (!container.Exists())
                {
                    await blobServiceClient.CreateBlobContainerAsync(dir);
                }
            }
        }
        public static void AddCorsRuleForAzure(this IApplicationBuilder app)
        {
            var corsRule = new CorsRule()
            {
                AllowedHeaders = new List<string> { "*" },
                AllowedMethods = CorsHttpMethods.Get,
                AllowedOrigins = new List<string> { "https://localhost:5001" },
                MaxAgeInSeconds = 3600,
            };

            string connectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();
            ServiceProperties serviceProperties = client.GetServiceProperties();
            CorsProperties corsSettings = serviceProperties.Cors;

            corsSettings.CorsRules.Add(corsRule);
            client.SetServiceProperties(serviceProperties);
        }

        public static IServiceCollection SetupJtwAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsSection = configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services
                .AddAuthentication(auth =>
                {
                    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(bearer =>
                {
                    bearer.RequireHttpsMetadata = false;
                    bearer.SaveToken = true;
                    bearer.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            return services;
        }
    }
}
