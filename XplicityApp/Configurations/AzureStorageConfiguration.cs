using System;
using Microsoft.Extensions.Configuration;

namespace XplicityApp.Configurations
{
    public static class AzureStorageConfiguration
    {
        private static IConfiguration _configuration;
        
        public static void Configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        public static string GetConnectionString()
        {
            string connectionString;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            {
                var environmentVariableName = _configuration.GetValue<string>("AzureStorage:EnvironmentVariableName");
                connectionString = Environment.GetEnvironmentVariable(environmentVariableName);
            }
            else
            {
                connectionString = _configuration.GetValue<string>("AzureStorage:ConnectionString");
            }

            return connectionString;
        }
    }
}