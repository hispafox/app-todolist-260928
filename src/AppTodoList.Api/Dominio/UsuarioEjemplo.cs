namespace AppTodoList.Api.Dominio;

public sealed class UsuarioEjemplo
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public ICollection<Tarea> Tareas { get; } = [];
}