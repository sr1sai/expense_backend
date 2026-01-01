namespace Domain
{
    public class HealthStatus
    {
        public string Service { get; set; }
        public string Status { get; set; }
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }
    }

    public class HealthCheckResult
    {
        public HealthStatus Backend { get; set; }
        public HealthStatus Database { get; set; }
        public string OverallStatus { get; set; }
    }
}
