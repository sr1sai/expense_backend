using Domain;

namespace Services.Contracts
{
    public interface IHealthService
    {
        Response<HealthCheckResult> GetHealthStatus();
        Response<HealthStatus> GetBackendStatus();
        Response<HealthStatus> GetDatabaseStatus();
    }
}
