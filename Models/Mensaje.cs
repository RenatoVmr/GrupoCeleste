using System.ComponentModel.DataAnnotations;

namespace GrupoCeleste.Models
{
    public class Mensaje
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Asunto { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string MensajeTexto { get; set; } = string.Empty;

        [Required]
        public DateTime Fecha { get; set; } = DateTime.Now;

        public string? Telefono { get; set; }

        public bool Leido { get; set; } = false;

        public DateTime? FechaLectura { get; set; }
    }
}