using Domain;

namespace Repositories.Contracts
{
    public interface IHealthRepository
    {
        bool CheckDatabaseConnection();
        string GetDatabaseVersion();
    }
}
