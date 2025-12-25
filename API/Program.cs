using Backend;

var builder = WebApplication.CreateBuilder(args);

// Configure port BEFORE building (required for Render)
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

// Health check endpoint for container orchestration
app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }));

app.Run();
