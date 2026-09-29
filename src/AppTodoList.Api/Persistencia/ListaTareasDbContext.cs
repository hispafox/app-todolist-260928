using AppTodoList.Api.Dominio;
using Microsoft.EntityFrameworkCore;

namespace AppTodoList.Api.Persistencia;

public sealed class ListaTareasDbContext(DbContextOptions<ListaTareasDbContext> opciones)
    : DbContext(opciones)
{
    public DbSet<Tarea> Tareas => Set<Tarea>();
    public DbSet<UsuarioEjemplo> Usuarios => Set<UsuarioEjemplo>();

    protected override void OnModelCreating(ModelBuilder modelo)
    {
        modelo.Entity<Tarea>(entidad =>
        {
            entidad.Property(tarea => tarea.Titulo).HasMaxLength(200).IsRequired();
            entidad.Property(tarea => tarea.Estado).HasConversion<string>().HasMaxLength(20);
            entidad.Property(tarea => tarea.Prioridad).HasConversion<string>().HasMaxLength(20);
            entidad.Property(tarea => tarea.FechaInicio).HasColumnType("TEXT").IsRequired(false);
            entidad.Property(tarea => tarea.FechaFin).HasColumnType("TEXT").IsRequired(false);
            entidad.HasOne(tarea => tarea.Responsable)
                .WithMany(usuario => usuario.Tareas)
                .HasForeignKey(tarea => tarea.ResponsableId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelo.Entity<UsuarioEjemplo>(entidad =>
        {
            entidad.Property(usuario => usuario.Nombre).HasMaxLength(100).IsRequired();
            entidad.HasData(
                new UsuarioEjemplo { Id = 1, Nombre = "Alex" },
                new UsuarioEjemplo { Id = 2, Nombre = "Sam" },
                new UsuarioEjemplo { Id = 3, Nombre = "Taylor" });
        });
    }
}