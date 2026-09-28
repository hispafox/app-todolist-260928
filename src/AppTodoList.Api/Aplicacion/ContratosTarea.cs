using AppTodoList.Api.Dominio;

namespace AppTodoList.Api.Aplicacion;

public sealed record SolicitudTarea(string Titulo, PrioridadTarea Prioridad = PrioridadTarea.Media, int? ResponsableId = null);
public sealed record SolicitudEstado(EstadoTarea Estado);
public sealed record TareaRespuesta(
    int Id,
    string Titulo,
    EstadoTarea Estado,
    PrioridadTarea Prioridad,
    int? ResponsableId,
    string? Responsable);
public sealed record UsuarioRespuesta(int Id, string Nombre);