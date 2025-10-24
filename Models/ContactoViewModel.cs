using System.ComponentModel.DataAnnotations;

namespace GrupoCeleste.Models
{
    public class ContactoViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "El nombre debe tener entre 2 y 100 caracteres")]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Por favor ingresa un email válido")]
        [StringLength(150, ErrorMessage = "El email no puede exceder los 150 caracteres")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "Por favor ingresa un número de teléfono válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder los 20 caracteres")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El asunto es obligatorio")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "El asunto debe tener entre 5 y 200 caracteres")]
        [Display(Name = "Asunto")]
        public string Asunto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El mensaje es obligatorio")]
        [StringLength(1000, MinimumLength = 10, ErrorMessage = "El mensaje debe tener entre 10 y 1000 caracteres")]
        [Display(Name = "Mensaje")]
        public string Mensaje { get; set; } = string.Empty;
    }
}