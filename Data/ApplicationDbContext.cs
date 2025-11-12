using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GrupoCeleste.Models;

namespace GrupoCeleste.Data;

public class ApplicationDbContext : IdentityDbContext<Usuario>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Pelicula> Peliculas { get; set; }
    public DbSet<Mensaje> Mensajes { get; set; }
    public DbSet<Resena> Resenas { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Configuración para la tabla Mensajes
        builder.Entity<Mensaje>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Asunto).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Contenido).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.FechaEnvio).IsRequired();
            entity.Property(e => e.Leido).HasDefaultValue(false);
            
            // Índices para mejorar consultas
            entity.HasIndex(e => e.FechaEnvio);
            entity.HasIndex(e => e.Leido);
            entity.HasIndex(e => e.Email);
        });

        // Configuración para la tabla Resenas
        builder.Entity<Resena>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Calificacion).IsRequired();
            entity.Property(e => e.Comentario).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.FechaCreacion).IsRequired();
            
            // Relación con Pelicula
            entity.HasOne(e => e.Pelicula)
                  .WithMany()
                  .HasForeignKey(e => e.PeliculaId)
                  .OnDelete(DeleteBehavior.Cascade);
                  
            // Relación con Usuario
            entity.HasOne(e => e.Usuario)
                  .WithMany()
                  .HasForeignKey(e => e.UsuarioId)
                  .OnDelete(DeleteBehavior.Cascade);
            
            // Índices para mejorar consultas
            entity.HasIndex(e => e.PeliculaId);
            entity.HasIndex(e => e.UsuarioId);
            entity.HasIndex(e => e.FechaCreacion);
            
            // Restricción única: un usuario solo puede reseñar una película una vez
            entity.HasIndex(e => new { e.PeliculaId, e.UsuarioId }).IsUnique();
        });
    }
}
