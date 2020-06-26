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
            var environmentVariableName = _configuration.GetValue<string>("AzureStorage:EnvironmentVariableName");
            var connectionString = Environment.GetEnvironmentVariable(environmentVariableName);
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = _configuration.GetValue<string>("AzureStorage:ConnectionString");
            }

            return connectionString;
        }
    }
}