using Database.Contracts;
using Microsoft.Extensions.Configuration;

namespace Database
{
    public class DatabaseConfig : IDatabaseConfig
    {
        public IConfiguration Configuration { get; private set; }

        public DatabaseConfig(bool production)
        {
            var configBuilder = new ConfigurationBuilder();

            // Base path should be the Backend solution folder
            var backendRoot = Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..");
            configBuilder.SetBasePath(backendRoot);

            var settingsFolder = "DatabaseContext";
            var fileName = production ? "DatabaseSettings.prod.json" : "DatabaseSettings.json";
            var filePath = Path.Combine(settingsFolder, fileName);

            // Log this if you want to verify the path
            //Console.WriteLine($"Loading database config from: {Path.GetFullPath(Path.Combine(backendRoot, filePath))}");

            configBuilder.AddJsonFile(filePath, optional: false, reloadOnChange: true);
            Configuration = configBuilder.Build();
        }
    }
}
