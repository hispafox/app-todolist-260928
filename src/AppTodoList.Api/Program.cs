using System.Text.Json.Serialization;
using AppTodoList.Api.Aplicacion;
using AppTodoList.Api.Dominio;
using AppTodoList.Api.Persistencia;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var carpetaBaseDatos = Path.Combine(builder.Environment.ContentRootPath, "App_Data");
Directory.CreateDirectory(carpetaBaseDatos);
var rutaBaseDatos = builder.Configuration.GetConnectionString("ListaTareas")
    ?? $"Data Source={Path.Combine(carpetaBaseDatos, "tareas.db")}";

builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(opciones =>
    opciones.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddDbContext<ListaTareasDbContext>(opciones =>
    opciones.UseSqlite(rutaBaseDatos));
builder.Services.AddScoped<ServicioTareas>();
builder.Services.AddCors(opciones => opciones.AddDefaultPolicy(politica =>
    politica.WithOrigins("http://localhost:5173")
        .AllowAnyHeader()
        .AllowAnyMethod()));

var app = builder.Build();

await using (var alcance = app.Services.CreateAsyncScope())
{
    var contexto = alcance.ServiceProvider.GetRequiredService<ListaTareasDbContext>();
    await contexto.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors();
app.UseHttpsRedirection();

app.MapGet("/api/usuarios", async (ServicioTareas servicio, CancellationToken cancelacion) =>
    Results.Ok(await servicio.ListarUsuariosAsync(cancelacion)));

app.MapGet("/api/tareas", async (
    EstadoTarea? estado,
    ServicioTareas servicio,
    CancellationToken cancelacion) =>
    Results.Ok(await servicio.ListarAsync(estado, cancelacion)));

app.MapGet("/api/tareas/{id:int}", async (
    int id,
    ServicioTareas servicio,
    CancellationToken cancelacion) =>
{
    var tarea = await servicio.ObtenerAsync(id, cancelacion);
    return tarea is null ? Results.NotFound() : Results.Ok(tarea);
});

app.MapPost("/api/tareas", async (
    SolicitudTarea solicitud,
    ServicioTareas servicio,
    CancellationToken cancelacion) =>
{
    try
    {
        var tarea = await servicio.CrearAsync(solicitud, cancelacion);
        return Results.Created($"/api/tareas/{tarea.Id}", tarea);
    }
    catch (ArgumentException error)
    {
        return ErrorValidacion(error.Message);
    }
});

app.MapPut("/api/tareas/{id:int}", async (
    int id,
    SolicitudTarea solicitud,
    ServicioTareas servicio,
    CancellationToken cancelacion) =>
{
    try
    {
        var tarea = await servicio.EditarAsync(id, solicitud, cancelacion);
        return tarea is null ? Results.NotFound() : Results.Ok(tarea);
    }
    catch (ArgumentException error)
    {
        return ErrorValidacion(error.Message);
    }
});

app.MapPut("/api/tareas/{id:int}/estado", async (
    int id,
    SolicitudEstado solicitud,
    ServicioTareas servicio,
    CancellationToken cancelacion) =>
{
    try
    {
        var tarea = await servicio.CambiarEstadoAsync(id, solicitud.Estado, cancelacion);
        return tarea is null ? Results.NotFound() : Results.Ok(tarea);
    }
    catch (ArgumentException error)
    {
        return ErrorValidacion(error.Message);
    }
});

app.MapDelete("/api/tareas/{id:int}", async (
    int id,
    ServicioTareas servicio,
    CancellationToken cancelacion) =>
    await servicio.EliminarAsync(id, cancelacion) ? Results.NoContent() : Results.NotFound());

app.Run();

static IResult ErrorValidacion(string mensaje) =>
    Results.ValidationProblem(new Dictionary<string, string[]> { ["tarea"] = [mensaje] });

public partial class Program;
