namespace AppTodoList.Api.Dominio;

public sealed class Tarea
{
    public int Id { get; set; }
    public required string Titulo { get; set; }
    public EstadoTarea Estado { get; set; } = EstadoTarea.Pendiente;
    public PrioridadTarea Prioridad { get; set; } = PrioridadTarea.Media;
    public DateOnly? FechaInicio { get; set; }
    public DateOnly? FechaFin { get; set; }
    public int? ResponsableId { get; set; }
    public UsuarioEjemplo? Responsable { get; set; }
}