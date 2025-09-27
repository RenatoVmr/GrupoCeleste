using System.ComponentModel.DataAnnotations;

namespace GrupoCeleste.Models
{
    public class Pelicula
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200, ErrorMessage = "El título no puede exceder los 200 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        [StringLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El director es requerido")]
        [StringLength(100, ErrorMessage = "El nombre del director no puede exceder los 100 caracteres")]
        public string Director { get; set; } = string.Empty;

        [Required(ErrorMessage = "El género es requerido")]
        [StringLength(50, ErrorMessage = "El género no puede exceder los 50 caracteres")]
        public string Genero { get; set; } = string.Empty;

        [Required(ErrorMessage = "El año de lanzamiento es requerido")]
        [Range(1895, 2030, ErrorMessage = "El año debe estar entre 1895 y 2030")]
        public int AnioLanzamiento { get; set; }

        [Range(1, 300, ErrorMessage = "La duración debe estar entre 1 y 300 minutos")]
        public int DuracionMinutos { get; set; }

        [Range(0.0, 10.0, ErrorMessage = "La calificación debe estar entre 0.0 y 10.0")]
        public decimal Calificacion { get; set; }

        public string? ImagenUrl { get; set; }

        public string? TrailerUrl { get; set; }

        [StringLength(500, ErrorMessage = "Los actores no pueden exceder los 500 caracteres")]
        public string? Actores { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public bool EsVisible { get; set; } = true;
    }
}