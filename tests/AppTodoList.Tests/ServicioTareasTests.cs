using AppTodoList.Api.Aplicacion;
using AppTodoList.Api.Dominio;
using AppTodoList.Api.Persistencia;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AppTodoList.Tests;

public sealed class ServicioTareasTests : IAsyncLifetime
{
    private SqliteConnection _conexion = null!;
    private ListaTareasDbContext _contexto = null!;
    private ServicioTareas _servicio = null!;

    public async Task InitializeAsync()
    {
        _conexion = new SqliteConnection("Data Source=:memory:");
        await _conexion.OpenAsync();
        var opciones = new DbContextOptionsBuilder<ListaTareasDbContext>()
            .UseSqlite(_conexion)
            .Options;
        _contexto = new ListaTareasDbContext(opciones);
        await _contexto.Database.MigrateAsync();
        _servicio = new ServicioTareas(_contexto);
    }

    public async Task DisposeAsync()
    {
        await _contexto.DisposeAsync();
        await _conexion.DisposeAsync();
    }

    [Fact]
    public async Task CrearTarea_UsaValoresInicialesYConservaElResponsableOpcional()
    {
        var tarea = await _servicio.CrearAsync(new SolicitudTarea("  Comprar leche  "), default);

        Assert.Equal("Comprar leche", tarea.Titulo);
        Assert.Equal(EstadoTarea.Pendiente, tarea.Estado);
        Assert.Equal(PrioridadTarea.Media, tarea.Prioridad);
        Assert.Null(tarea.ResponsableId);
    }

    [Fact]
    public async Task CrearTarea_RechazaTituloVacio()
    {
        var error = await Assert.ThrowsAsync<ArgumentException>(() =>
            _servicio.CrearAsync(new SolicitudTarea("  "), default));

        Assert.Contains("obligatorio", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CrearTarea_RechazaResponsableDesconocido()
    {
        await Assert.ThrowsAsync<ArgumentException>(() =>
            _servicio.CrearAsync(new SolicitudTarea("Tarea", ResponsableId: 999), default));
    }

    [Fact]
    public async Task Tareas_PermitenEdicionEstadoFiltroYEliminacion()
    {
        var pendiente = await _servicio.CrearAsync(new SolicitudTarea("Pendiente"), default);
        var completada = await _servicio.CrearAsync(
            new SolicitudTarea("Terminada", PrioridadTarea.Alta, 1), default);

        var actualizada = await _servicio.EditarAsync(
            pendiente.Id,
            new SolicitudTarea("Nueva tarea", PrioridadTarea.Baja, 2),
            default);
        var cambiada = await _servicio.CambiarEstadoAsync(completada.Id, EstadoTarea.Completada, default);
        var filtradas = await _servicio.ListarAsync(EstadoTarea.Completada, default);

        Assert.Equal("Nueva tarea", actualizada!.Titulo);
        Assert.Equal("Sam", actualizada.Responsable);
        Assert.Equal(EstadoTarea.Completada, cambiada!.Estado);
        Assert.Single(filtradas);
        Assert.Equal(completada.Id, filtradas[0].Id);
        Assert.True(await _servicio.EliminarAsync(pendiente.Id, default));
        Assert.Null(await _servicio.ObtenerAsync(pendiente.Id, default));
    }

    [Fact]
    public async Task Datos_SeRecuperanEnOtroContextoSobreLaMismaBase()
    {
        var creada = await _servicio.CrearAsync(new SolicitudTarea("Persistente"), default);
        var opciones = new DbContextOptionsBuilder<ListaTareasDbContext>()
            .UseSqlite(_conexion)
            .Options;

        await using var nuevoContexto = new ListaTareasDbContext(opciones);
        var servicioNuevo = new ServicioTareas(nuevoContexto);
        var recuperada = await servicioNuevo.ObtenerAsync(creada.Id, default);

        Assert.Equal("Persistente", recuperada!.Titulo);
    }
}