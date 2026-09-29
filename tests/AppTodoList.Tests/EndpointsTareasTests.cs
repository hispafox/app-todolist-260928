using System.Net;
using System.Net.Http.Json;
using AppTodoList.Api.Persistencia;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AppTodoList.Tests;

public sealed class EndpointsTareasTests : IAsyncLifetime
{
    private SqliteConnection _conexion = null!;
    private WebApplicationFactory<Program> _fabrica = null!;
    private HttpClient _cliente = null!;

    public async Task InitializeAsync()
    {
        _conexion = new SqliteConnection("Data Source=:memory:");
        await _conexion.OpenAsync();

        _fabrica = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            builder.ConfigureServices(servicios =>
            {
                servicios.RemoveAll<DbContextOptions<ListaTareasDbContext>>();
                servicios.RemoveAll<ListaTareasDbContext>();
                servicios.AddDbContext<ListaTareasDbContext>(opciones => opciones.UseSqlite(_conexion));
            }));
        _cliente = _fabrica.CreateClient();
    }

    public async Task DisposeAsync()
    {
        _cliente.Dispose();
        await _fabrica.DisposeAsync();
        await _conexion.DisposeAsync();
    }

    [Fact]
    public async Task CambiarEstado_ConEnteroFueraDeRango_Devuelve400ConValidationProblem()
    {
        var creacion = await _cliente.PostAsJsonAsync("/api/tareas", new { titulo = "Tarea de prueba" });
        creacion.EnsureSuccessStatusCode();
        var tareaCreada = await creacion.Content.ReadFromJsonAsync<TareaRespuestaDto>();

        var respuesta = await _cliente.PutAsJsonAsync(
            $"/api/tareas/{tareaCreada!.Id}/estado",
            new { estado = 5 });

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        var cuerpo = await respuesta.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        Assert.True(cuerpo!.ContainsKey("errors"));
    }

    [Fact]
    public async Task CambiarEstado_ConStringNoReconocido_Devuelve400()
    {
        var creacion = await _cliente.PostAsJsonAsync("/api/tareas", new { titulo = "Tarea de prueba" });
        creacion.EnsureSuccessStatusCode();
        var tareaCreada = await creacion.Content.ReadFromJsonAsync<TareaRespuestaDto>();

        var contenido = new StringContent("{\"estado\": \"Cancelada\"}", System.Text.Encoding.UTF8, "application/json");
        var respuesta = await _cliente.PutAsync($"/api/tareas/{tareaCreada!.Id}/estado", contenido);

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [Fact]
    public async Task Fechas_CrearConsultarYEditar_SeConservanEnLaPersistencia()
    {
        var inicio = new DateOnly(2026, 9, 29);
        var fin = new DateOnly(2026, 10, 1);
        var creacion = await _cliente.PostAsJsonAsync("/api/tareas", new
        {
            titulo = "Tarea con fechas",
            fechaInicio = inicio,
            fechaFin = fin
        });

        creacion.EnsureSuccessStatusCode();
        var creada = await creacion.Content.ReadFromJsonAsync<TareaFechasRespuestaDto>();
        Assert.Equal(inicio, creada!.FechaInicio);
        Assert.Equal(fin, creada.FechaFin);

        var consulta = await _cliente.GetFromJsonAsync<TareaFechasRespuestaDto>($"/api/tareas/{creada.Id}");
        Assert.Equal(inicio, consulta!.FechaInicio);
        Assert.Equal(fin, consulta.FechaFin);

        var edicion = await _cliente.PutAsJsonAsync($"/api/tareas/{creada.Id}", new
        {
            titulo = "Tarea editada",
            fechaInicio = inicio,
            fechaFin = (DateOnly?)null
        });
        edicion.EnsureSuccessStatusCode();
        var actualizada = await edicion.Content.ReadFromJsonAsync<TareaFechasRespuestaDto>();

        Assert.Equal(inicio, actualizada!.FechaInicio);
        Assert.Null(actualizada.FechaFin);
        var recuperada = await _cliente.GetFromJsonAsync<TareaFechasRespuestaDto>($"/api/tareas/{creada.Id}");
        Assert.Equal(inicio, recuperada!.FechaInicio);
        Assert.Null(recuperada.FechaFin);
    }

    [Fact]
    public async Task Fechas_FinSinInicio_Devuelve400ConErrorComprensible()
    {
        var respuesta = await _cliente.PostAsJsonAsync("/api/tareas", new
        {
            titulo = "Tarea inválida",
            fechaFin = new DateOnly(2026, 10, 1)
        });

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        var cuerpo = await respuesta.Content.ReadAsStringAsync();
        Assert.Contains("requiere una fecha de inicio", cuerpo, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Fechas_InicioPosteriorAlFin_Devuelve400ConErrorComprensible()
    {
        var respuesta = await _cliente.PostAsJsonAsync("/api/tareas", new
        {
            titulo = "Tarea inválida",
            fechaInicio = new DateOnly(2026, 10, 2),
            fechaFin = new DateOnly(2026, 10, 1)
        });

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
        var cuerpo = await respuesta.Content.ReadAsStringAsync();
        Assert.Contains("posterior", cuerpo, StringComparison.OrdinalIgnoreCase);
    }

    private sealed record TareaRespuestaDto(int Id);
    private sealed record TareaFechasRespuestaDto(int Id, DateOnly? FechaInicio, DateOnly? FechaFin);
}
