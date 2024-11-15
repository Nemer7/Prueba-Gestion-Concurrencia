using Microsoft.EntityFrameworkCore;
using Prueba_Gestion_Concurrencia.BackgroundServices;
using Prueba_Gestion_Concurrencia.Data;
using Prueba_Gestion_Concurrencia.Events;
using Prueba_Gestion_Concurrencia.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de la cadena de conexi�n
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Configuraci�n del DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configuraci�n de servicios
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddHostedService<ReservationEventWorker>();
builder.Services.AddSingleton<IEventBus, InMemoryEventBus>();
builder.Services.AddControllers();

// Configuraci�n de Swagger
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
        c.RoutePrefix = "swagger"; // Cambia esto a string.Empty si quieres que Swagger est� en la ra�z
    });
}

app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();