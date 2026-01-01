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
            
            // Add secret database configuration in development mode
            if (!production)
            {
                configBuilder.AddJsonFile("secret.database.json", optional: false, reloadOnChange: true);
            }
            
            var config = configBuilder.Build();
            
            // Build the actual connection string from template
            var connectionTemplate = config["ConnectionStrings:ConnectionTemplate"];
            
            if (!string.IsNullOrEmpty(connectionTemplate))
            {
                string connectionString;
                
                if (production)
                {
                    // In production, use environment variables
                    connectionString = connectionTemplate
                        .Replace("{HOST}", Environment.GetEnvironmentVariable("HOST") ?? "")
                        .Replace("{PORT}", Environment.GetEnvironmentVariable("PORT_DB") ?? "5432")
                        .Replace("{DATABASE}", Environment.GetEnvironmentVariable("DATABASE") ?? "")
                        .Replace("{USERNAME}", Environment.GetEnvironmentVariable("USERNAME") ?? "")
                        .Replace("{PASSWORD}", Environment.GetEnvironmentVariable("PASSWORD") ?? "")
                        .Replace("{SSL_MODE}", Environment.GetEnvironmentVariable("SSL_MODE") ?? "Require")
                        .Replace("{COMMAND_TIMEOUT}", Environment.GetEnvironmentVariable("COMMAND_TIMEOUT") ?? "30")
                        .Replace("{APPLICATION_NAME}", Environment.GetEnvironmentVariable("APPLICATION_NAME") ?? "expense_backend");
                }
                else
                {
                    // In development, use values from secret.database.json
                    connectionString = connectionTemplate
                        .Replace("{HOST}", config["Host"] ?? "")
                        .Replace("{PORT}", config["Port"] ?? "5432")
                        .Replace("{DATABASE}", config["Database"] ?? "")
                        .Replace("{USERNAME}", config["Username"] ?? "")
                        .Replace("{PASSWORD}", config["Password"] ?? "")
                        .Replace("{SSL_MODE}", config["SSLMode"] ?? "Require")
                        .Replace("{COMMAND_TIMEOUT}", config["CommandTimeout"] ?? "30")
                        .Replace("{APPLICATION_NAME}", config["ApplicationName"] ?? "expense_backend");
                }
                
                // Add the built connection string to the configuration
                configBuilder.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    { "ConnectionStrings:DefaultConnection", connectionString }
                });
            }
            
            Configuration = configBuilder.Build();
        }
    }
}
