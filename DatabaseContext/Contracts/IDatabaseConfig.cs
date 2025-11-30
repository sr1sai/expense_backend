using Microsoft.Extensions.Configuration;

namespace Database.Contracts
{
    public interface IDatabaseConfig
    {
        IConfiguration Configuration { get; }
    }
}
