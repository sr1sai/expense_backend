using Domain;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class HealthController : ControllerBase
    {
        private readonly IHealthService _healthService;
        private readonly ILogger<HealthController> _logger;

        public HealthController(IHealthService healthService, ILogger<HealthController> logger)
        {
            _healthService = healthService;
            _logger = logger;
        }

        [HttpGet]
        [ActionName("Status")]
        public Response<HealthCheckResult> GetHealthStatus()
        {
            _logger.LogInformation("Health check requested - GetHealthStatus at {Timestamp}", DateTime.UtcNow);
            try
            {
                var result = _healthService.GetHealthStatus();
                _logger.LogInformation("Health check completed - Status: {Status}, Overall: {OverallStatus}", 
                    result.status, result.data?.OverallStatus);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetHealthStatus");
                throw;
            }
        }

        [HttpGet]
        [ActionName("Backend")]
        public Response<HealthStatus> GetBackendStatus()
        {
            _logger.LogInformation("Backend health check requested at {Timestamp}", DateTime.UtcNow);
            try
            {
                var result = _healthService.GetBackendStatus();
                _logger.LogInformation("Backend health check completed - Status: {Status}", result.status);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetBackendStatus");
                throw;
            }
        }

        [HttpGet]
        [ActionName("Database")]
        public Response<HealthStatus> GetDatabaseStatus()
        {
            _logger.LogInformation("Database health check requested at {Timestamp}", DateTime.UtcNow);
            try
            {
                var result = _healthService.GetDatabaseStatus();
                _logger.LogInformation("Database health check completed - Status: {Status}", result.status);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during GetDatabaseStatus");
                throw;
            }
        }
    }
}
