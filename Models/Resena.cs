using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrupoCeleste.Models;

public class Resena
{
    public int Id { get; set; }

    [Required]
    public int PeliculaId { get; set; }

    [Required]
    public string UsuarioId { get; set; } = string.Empty;

    [Required(ErrorMessage = "La calificación es obligatoria")]
    [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5 estrellas")]
    public int Calificacion { get; set; }

    [Required(ErrorMessage = "El comentario es obligatorio")]
    [StringLength(1000, ErrorMessage = "El comentario no puede exceder los 1000 caracteres")]
    [MinLength(10, ErrorMessage = "El comentario debe tener al menos 10 caracteres")]
    public string Comentario { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    // Navigation properties
    [ForeignKey("PeliculaId")]
    public virtual Pelicula? Pelicula { get; set; }

    [ForeignKey("UsuarioId")]
    public virtual Usuario? Usuario { get; set; }
}