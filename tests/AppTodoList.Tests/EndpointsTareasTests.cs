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

    private sealed record TareaRespuestaDto(int Id);
}
