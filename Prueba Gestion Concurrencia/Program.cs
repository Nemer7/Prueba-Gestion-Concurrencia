using Microsoft.EntityFrameworkCore;
using Prueba_Gestion_Concurrencia.BackgroundServices;
using Prueba_Gestion_Concurrencia.Data;
using Prueba_Gestion_Concurrencia.Events;
using Prueba_Gestion_Concurrencia.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuración del DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configuración de servicios
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddHostedService<ReservationEventWorker>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();
builder.Services.AddControllers();

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = "swagger"; // Cambia esto a string.Empty si quieres que Swagger esté en la raíz
    });
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();