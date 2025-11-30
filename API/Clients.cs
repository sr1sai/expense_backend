namespace Backend
{
    public static class Clients
    {
        public static IServiceCollection AddClients(this IServiceCollection services,IConfiguration configuration, bool production = false)
        {
            var clientsSection = configuration.GetSection("Clients");

            var allowedOrigins = clientsSection.GetChildren()
                                .Select(client => client.Value)
                                .Where(url => !string.IsNullOrEmpty(url))
                                .ToArray();
            services.AddCors(options =>
            {
                options.AddPolicy("AllowedClients", policy => {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
            return services;
        }
    }
}
