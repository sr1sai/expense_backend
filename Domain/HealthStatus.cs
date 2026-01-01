namespace Domain
{
    public class HealthStatus
    {
        public string Service { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class HealthCheckResult
    {
        public HealthStatus Backend { get; set; } = null!;
        public HealthStatus Database { get; set; } = null!;
        public string OverallStatus { get; set; } = string.Empty;
    }
}
