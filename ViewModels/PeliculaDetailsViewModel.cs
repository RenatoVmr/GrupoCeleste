using GrupoCeleste.Models;
using System.ComponentModel.DataAnnotations;

namespace GrupoCeleste.ViewModels;

public class PeliculaDetailsViewModel
{
    public Pelicula Pelicula { get; set; } = null!;
    public List<ResenaViewModel> Resenas { get; set; } = new();
    public double PromedioCalificaciones { get; set; }
    public int TotalResenas { get; set; }
    public bool UsuarioPuedeResenar { get; set; }
    public bool UsuarioYaReseno { get; set; }
    public NuevaResenaViewModel? NuevaResena { get; set; }
}

public class ResenaViewModel
{
    public int Id { get; set; }
    public int Calificacion { get; set; }
    public string Comentario { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; }
    public string NombreUsuario { get; set; } = string.Empty;
}

public class NuevaResenaViewModel
{
    public int PeliculaId { get; set; }

    [Required(ErrorMessage = "La calificación es obligatoria")]
    [Range(1, 5, ErrorMessage = "La calificación debe estar entre 1 y 5 estrellas")]
    public int Calificacion { get; set; }

    [Required(ErrorMessage = "El comentario es obligatorio")]
    [StringLength(1000, ErrorMessage = "El comentario no puede exceder los 1000 caracteres")]
    [MinLength(10, ErrorMessage = "El comentario debe tener al menos 10 caracteres")]
    public string Comentario { get; set; } = string.Empty;
}