using Microsoft.EntityFrameworkCore;
using BusTicketDemoApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurar puerto para Render (usa variable de entorno PORT o 10000 por defecto)
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// Add services to the container.
builder.Services.AddControllers();

// Configurar connection string para Render
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Verificar si estamos usando una variable de entorno para la conexión
if (string.IsNullOrEmpty(connectionString) || connectionString.Contains("${DATABASE_URL}"))
{
    // Usar variable de entorno para la base de datos en Render
    connectionString = Environment.GetEnvironmentVariable("DATABASE_URL") ?? 
        "Server=localhost;Database=BusticketDb;User Id=sa;Password=YourStrongPassword;TrustServerCertificate=true;";
}

// Add Entity Framework
builder.Services.AddDbContext<BusTicketDbContext>(options =>
    options.UseSqlServer(connectionString));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Apply pending migrations and seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BusTicketDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    
    try
    {
        // Apply any pending migrations
        context.Database.Migrate();
        logger.LogInformation("Database migration completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database.");
        throw;
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // En producción también habilitamos Swagger para facilitar testing
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BusTicket API V1");
        c.RoutePrefix = "swagger";
    });
}

// Solo usar HTTPS redirect en desarrollo, en Fly.io se maneja automáticamente
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
