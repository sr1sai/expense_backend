namespace Backend
{
    public static class Clients
    {
        public static IServiceCollection AddClients(
            this IServiceCollection services,
            IConfiguration configuration,
            bool production = false)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowedClients", policy =>
                {
                    policy
                        .AllowAnyOrigin()   // Allow all origins
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return services;
        }

    }
}
