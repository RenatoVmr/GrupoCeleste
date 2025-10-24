using System.ComponentModel.DataAnnotations;

namespace GrupoCeleste.Models
{
    public class Mensaje
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Asunto { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Contenido { get; set; } = string.Empty;

        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        public bool Leido { get; set; } = false;
    }
}