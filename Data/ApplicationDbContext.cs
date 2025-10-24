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
    }
}
