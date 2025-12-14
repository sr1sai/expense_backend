using Database;
using Database.Contracts;
using Repositories;
using Repositories.Contracts;
using Services;
using Services.Contracts;


namespace Backend
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddProjectDependencies(this IServiceCollection services, bool production = false)
        {
            // Register DatabaseConfig as IDatabaseConfig
            services.AddSingleton<IDatabaseConfig>(provider => new DatabaseConfig(production));

            // Register your database context implementation
            services.AddScoped<IDatabaseContext, DatabaseContext>();

            // Register repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();

            // Register services
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITransactionService, TransactionService>();

            return services;
        }
    }
}
