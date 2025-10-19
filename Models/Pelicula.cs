using System.ComponentModel.DataAnnotations;

namespace GrupoCeleste.Models;

public class Pelicula
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(200)]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La descripción es obligatoria")]
    [StringLength(1000)]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El director es obligatorio")]
    [StringLength(100)]
    public string Director { get; set; } = string.Empty;

    [Required(ErrorMessage = "El género es obligatorio")]
    [StringLength(50)]
    public string Genero { get; set; } = string.Empty;

    [Required(ErrorMessage = "El año de lanzamiento es obligatorio")]
    [Range(1888, 2100, ErrorMessage = "El año debe estar entre 1888 y 2100")]
    public int AnioLanzamiento { get; set; }

    [Required(ErrorMessage = "La duración es obligatoria")]
    [Range(1, 500, ErrorMessage = "La duración debe estar entre 1 y 500 minutos")]
    public int DuracionMinutos { get; set; }

    [Range(0, 10, ErrorMessage = "La calificación debe estar entre 0 y 10")]
    public double Calificacion { get; set; }

    [StringLength(500)]
    public string Actores { get; set; } = string.Empty;

    [StringLength(500)]
    public string ImagenUrl { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
