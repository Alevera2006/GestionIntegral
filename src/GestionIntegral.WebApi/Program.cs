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
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "GestionIntegral API";
        options.InjectStylesheet("/swagger/custom-swagger.css");
        options.InjectJavascript("/swagger/custom-swagger.js");
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ErrorHandlingMiddleware>();

app.MapControllers();

app.Run();
