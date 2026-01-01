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

        public HealthController(IHealthService healthService)
        {
            _healthService = healthService;
        }

        [HttpGet]
        [ActionName("Status")]
        public Response<HealthCheckResult> GetHealthStatus()
        {
            return _healthService.GetHealthStatus();
        }

        [HttpGet]
        [ActionName("Backend")]
        public Response<HealthStatus> GetBackendStatus()
        {
            return _healthService.GetBackendStatus();
        }

        [HttpGet]
        [ActionName("Database")]
        public Response<HealthStatus> GetDatabaseStatus()
        {
            return _healthService.GetDatabaseStatus();
        }
    }
}
