using Backend;
using Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Configure port - use PORT env variable in production, or launchSettings.json in development
if (builder.Environment.IsProduction())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    builder.WebHost.UseUrls($"http://0.0.0.0:{port}");
}
else
{
    // In development, let launchSettings.json control the port, or use 8080 if running without it
    var port = Environment.GetEnvironmentVariable("PORT");
    if (!string.IsNullOrEmpty(port))
    {
        builder.WebHost.UseUrls($"http://localhost:{port}");
    }
}

// Add services to the container.
// Learning more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//custom middlewares
builder.Services.AddProjectDependencies(builder.Environment.IsProduction(), builder.Configuration);
builder.Services.AddClients(builder.Configuration, builder.Environment.IsProduction());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowedClients");

// Comment out HTTPS redirection (Render handles SSL at load balancer)
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Health check endpoint for container orchestration using the full HealthService
app.MapGet("/health", (IHealthService healthService) =>
{
    var healthStatus = healthService.GetHealthStatus();
    
    if (healthStatus.status && healthStatus.data.OverallStatus == "Healthy")
    {
        return Results.Ok(healthStatus);
    }
    
    return Results.Json(healthStatus, statusCode: 503);
});

app.Run();
