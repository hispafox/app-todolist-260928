using AppTodoList.Api.Dominio;

namespace AppTodoList.Api.Aplicacion;

public sealed record SolicitudTarea(
    string Titulo,
    PrioridadTarea Prioridad = PrioridadTarea.Media,
    int? ResponsableId = null,
    DateOnly? FechaInicio = null,
    DateOnly? FechaFin = null);
public sealed record SolicitudEstado(EstadoTarea Estado);
public sealed record TareaRespuesta(
    int Id,
    string Titulo,
    EstadoTarea Estado,
    PrioridadTarea Prioridad,
    int? ResponsableId,
    string? Responsable,
    DateOnly? FechaInicio,
    DateOnly? FechaFin);
public sealed record UsuarioRespuesta(int Id, string Nombre);