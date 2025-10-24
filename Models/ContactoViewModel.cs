using System.ComponentModel.DataAnnotations;

namespace GrupoCeleste.Models
{
    public class ContactoViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "Formato de teléfono inválido")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El asunto es obligatorio")]
        [Display(Name = "Asunto")]
        public string Asunto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El mensaje es obligatorio")]
        [Display(Name = "Mensaje")]
        [MinLength(10, ErrorMessage = "El mensaje debe tener al menos 10 caracteres")]
        public string Mensaje { get; set; } = string.Empty;
    }
}