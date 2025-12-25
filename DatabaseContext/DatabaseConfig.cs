using Database.Contracts;
using Microsoft.Extensions.Configuration;

namespace Database
{
    public class DatabaseConfig : IDatabaseConfig
    {
        public IConfiguration Configuration { get; private set; }

        public DatabaseConfig(bool production, IConfiguration configuration)
        {
            // Create a new configuration that includes the database settings file
            var configBuilder = new ConfigurationBuilder();
            
            // Start with existing configuration (appsettings.json, environment variables, etc.)
            configBuilder.AddConfiguration(configuration);
            
            // Add database-specific settings
            var fileName = production ? "DatabaseSettings.prod.json" : "DatabaseSettings.json";
            configBuilder.AddJsonFile(fileName, optional: false, reloadOnChange: true);
            
            Configuration = configBuilder.Build();
        }
    }
}
