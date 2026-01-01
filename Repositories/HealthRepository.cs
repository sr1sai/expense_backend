using Database.Contracts;
using Domain;
using Repositories.Contracts;

namespace Repositories
{
    public class HealthRepository : IHealthRepository
    {
        private readonly IDatabaseContext _databaseContext;

        public HealthRepository(IDatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public bool CheckDatabaseConnection()
        {
            try
            {
                var query = new ParameterisedQuery
                {
                    query = "SELECT 1",
                    parameters = new Dictionary<string, object>()
                };

                var result = _databaseContext.ExecuteScalar(query);
                return result != null;
            }
            catch
            {
                return false;
            }
        }

        public string GetDatabaseVersion()
        {
            try
            {
                var query = new ParameterisedQuery
                {
                    query = "SELECT version()",
                    parameters = new Dictionary<string, object>()
                };

                var result = _databaseContext.ExecuteScalar(query);
                return result?.ToString() ?? "Unknown";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
