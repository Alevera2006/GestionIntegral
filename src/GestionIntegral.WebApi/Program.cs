using GestionIntegral.Infrastructure.DependencyInjection;
using GestionIntegral.WebApi;
using GestionIntegral.WebApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configuración de archivos
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("config/appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"config/appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddJsonFile("config/connectionStrings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

Console.Write("New SuperAdmin password: ");
string password = Console.ReadLine()
    ?? throw new InvalidOperationException("Password is required.");

Console.WriteLine(BCrypt.Net.BCrypt.HashPassword(password));

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
