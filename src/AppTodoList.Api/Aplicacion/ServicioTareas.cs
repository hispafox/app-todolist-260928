using AppTodoList.Api.Dominio;
using AppTodoList.Api.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace AppTodoList.Api.Aplicacion;

public sealed class ServicioTareas(ListaTareasDbContext contexto)
{
    public async Task<IReadOnlyList<TareaRespuesta>> ListarAsync(
        EstadoTarea? estado,
        CancellationToken cancelacion)
    {
        var consulta = contexto.Tareas.Include(tarea => tarea.Responsable).AsQueryable();
        if (estado.HasValue)
        {
            consulta = consulta.Where(tarea => tarea.Estado == estado.Value);
        }

        var tareas = await consulta.OrderBy(tarea => tarea.Id).ToListAsync(cancelacion);
        return tareas.Select(Convertir).ToArray();
    }

    public async Task<IReadOnlyList<UsuarioRespuesta>> ListarUsuariosAsync(CancellationToken cancelacion) =>
        await contexto.Usuarios
            .OrderBy(usuario => usuario.Nombre)
            .Select(usuario => new UsuarioRespuesta(usuario.Id, usuario.Nombre))
            .ToListAsync(cancelacion);

    public async Task<TareaRespuesta?> ObtenerAsync(int id, CancellationToken cancelacion)
    {
        var tarea = await contexto.Tareas
            .Include(elemento => elemento.Responsable)
            .SingleOrDefaultAsync(elemento => elemento.Id == id, cancelacion);
        return tarea is null ? null : Convertir(tarea);
    }

    public async Task<TareaRespuesta> CrearAsync(SolicitudTarea solicitud, CancellationToken cancelacion)
    {
        await ValidarAsync(solicitud, cancelacion);
        var tarea = new Tarea
        {
            Titulo = solicitud.Titulo.Trim(),
            Prioridad = solicitud.Prioridad,
            FechaInicio = solicitud.FechaInicio,
            FechaFin = solicitud.FechaFin,
            ResponsableId = solicitud.ResponsableId
        };
        contexto.Tareas.Add(tarea);
        await contexto.SaveChangesAsync(cancelacion);
        return (await ObtenerAsync(tarea.Id, cancelacion))!;
    }

    public async Task<TareaRespuesta?> EditarAsync(
        int id,
        SolicitudTarea solicitud,
        CancellationToken cancelacion)
    {
        var tarea = await contexto.Tareas.FindAsync([id], cancelacion);
        if (tarea is null)
        {
            return null;
        }

        await ValidarAsync(solicitud, cancelacion);
        tarea.Titulo = solicitud.Titulo.Trim();
        tarea.Prioridad = solicitud.Prioridad;
        tarea.FechaInicio = solicitud.FechaInicio;
        tarea.FechaFin = solicitud.FechaFin;
        tarea.ResponsableId = solicitud.ResponsableId;
        await contexto.SaveChangesAsync(cancelacion);
        return await ObtenerAsync(id, cancelacion);
    }

    public async Task<TareaRespuesta?> CambiarEstadoAsync(
        int id,
        EstadoTarea estado,
        CancellationToken cancelacion)
    {
        if (!Enum.IsDefined(estado))
        {
            throw new ArgumentException("El estado indicado no es válido.");
        }

        var tarea = await contexto.Tareas.FindAsync([id], cancelacion);
        if (tarea is null)
        {
            return null;
        }

        tarea.Estado = estado;
        await contexto.SaveChangesAsync(cancelacion);
        return await ObtenerAsync(id, cancelacion);
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken cancelacion)
    {
        var tarea = await contexto.Tareas.FindAsync([id], cancelacion);
        if (tarea is null)
        {
            return false;
        }

        contexto.Tareas.Remove(tarea);
        await contexto.SaveChangesAsync(cancelacion);
        return true;
    }

    private async Task ValidarAsync(SolicitudTarea solicitud, CancellationToken cancelacion)
    {
        if (solicitud.FechaFin.HasValue && !solicitud.FechaInicio.HasValue)
        {
            throw new ArgumentException("La fecha de fin requiere una fecha de inicio.");
        }

        if (solicitud.FechaInicio > solicitud.FechaFin)
        {
            throw new ArgumentException("La fecha de inicio no puede ser posterior a la fecha de fin.");
        }

        if (string.IsNullOrWhiteSpace(solicitud.Titulo))
        {
            throw new ArgumentException("El título es obligatorio.");
        }

        if (solicitud.Titulo.Trim().Length > 200)
        {
            throw new ArgumentException("El título no puede superar los 200 caracteres.");
        }

        if (!Enum.IsDefined(solicitud.Prioridad))
        {
            throw new ArgumentException("La prioridad indicada no es válida.");
        }

        if (solicitud.ResponsableId is int responsableId &&
            !await contexto.Usuarios.AnyAsync(usuario => usuario.Id == responsableId, cancelacion))
        {
            throw new ArgumentException("El responsable debe pertenecer a la lista de usuarios disponibles.");
        }
    }

    private static TareaRespuesta Convertir(Tarea tarea) => new(
        tarea.Id,
        tarea.Titulo,
        tarea.Estado,
        tarea.Prioridad,
        tarea.ResponsableId,
        tarea.Responsable?.Nombre,
        tarea.FechaInicio,
        tarea.FechaFin);
}