using Domain;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class HealthService : IHealthService
    {
        private readonly IHealthRepository _healthRepository;

        public HealthService(IHealthRepository healthRepository)
        {
            _healthRepository = healthRepository;
        }

        public Response<HealthCheckResult> GetHealthStatus()
        {
            Response<HealthCheckResult> response = new Response<HealthCheckResult>();
            try
            {
                var backendStatus = GetBackendStatus();
                var databaseStatus = GetDatabaseStatus();

                var healthCheck = new HealthCheckResult
                {
                    Backend = backendStatus.data,
                    Database = databaseStatus.data,
                    OverallStatus = (backendStatus.data.Status == "Healthy" && databaseStatus.data.Status == "Healthy") 
                        ? "Healthy" 
                        : "Unhealthy"
                };

                response.status = true;
                response.data = healthCheck;
                response.message = $"Overall system status: {healthCheck.OverallStatus}";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                response.data = null;
            }
            return response;
        }

        public Response<HealthStatus> GetBackendStatus()
        {
            Response<HealthStatus> response = new Response<HealthStatus>();
            try
            {
                var backendHealth = new HealthStatus
                {
                    Service = "Backend API",
                    Status = "Healthy",
                    Timestamp = DateTime.UtcNow,
                    Message = "Backend is running successfully"
                };

                response.status = true;
                response.data = backendHealth;
                response.message = "Backend status retrieved successfully";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                response.data = new HealthStatus
                {
                    Service = "Backend API",
                    Status = "Unhealthy",
                    Timestamp = DateTime.UtcNow,
                    Message = ex.Message
                };
            }
            return response;
        }

        public Response<HealthStatus> GetDatabaseStatus()
        {
            Response<HealthStatus> response = new Response<HealthStatus>();
            try
            {
                bool isConnected = _healthRepository.CheckDatabaseConnection();
                string version = isConnected ? _healthRepository.GetDatabaseVersion() : "N/A";

                var databaseHealth = new HealthStatus
                {
                    Service = "PostgreSQL Database",
                    Status = isConnected ? "Healthy" : "Unhealthy",
                    Timestamp = DateTime.UtcNow,
                    Message = isConnected ? $"Database connected. Version: {version}" : "Unable to connect to database"
                };

                response.status = true;
                response.data = databaseHealth;
                response.message = "Database status retrieved successfully";
            }
            catch (Exception ex)
            {
                response.status = false;
                response.message = ex.Message;
                response.data = new HealthStatus
                {
                    Service = "PostgreSQL Database",
                    Status = "Unhealthy",
                    Timestamp = DateTime.UtcNow,
                    Message = ex.Message
                };
            }
            return response;
        }
    }
}
