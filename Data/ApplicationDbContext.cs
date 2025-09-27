using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GrupoCeleste.Models;

namespace GrupoCeleste.Data
{
    public class ApplicationDbContext : IdentityDbContext<Usuario>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Pelicula> Peliculas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configuración para Pelicula
            builder.Entity<Pelicula>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Titulo).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Descripcion).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Director).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Genero).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Calificacion).HasPrecision(3, 1);
                entity.Property(e => e.ImagenUrl).HasMaxLength(500);
                entity.Property(e => e.TrailerUrl).HasMaxLength(500);
                entity.Property(e => e.Actores).HasMaxLength(500);
            });

            // Configuración para Usuario (extendiendo IdentityUser)
            builder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Apellido).IsRequired().HasMaxLength(50);
            });
        }
    }
}