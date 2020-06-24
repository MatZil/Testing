using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace XplicityApp.Configurations
{
    public static class AzureStorageConfiguration
    {
        private static IServiceProvider _serviceProvider;
        
        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public static string GetConnectionString()
        {
            string connectionString;
            var configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                var environmentVariableName = configuration.GetValue<string>("AzureStorage:EnvironmentVariableName");
                connectionString = Environment.GetEnvironmentVariable(environmentVariableName);
            }
            else
            {
                connectionString = configuration.GetValue<string>("AzureStorage:ConnectionString");
            }

            return connectionString;
        }
    }
}