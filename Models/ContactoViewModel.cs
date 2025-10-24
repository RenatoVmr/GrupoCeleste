using System.ComponentModel.DataAnnotations;

namespace GrupoCeleste.Models
{
    public class ContactoViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "Ingrese un email válido")]
        [Display(Name = "Correo Electrónico")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "El asunto es obligatorio")]
        [Display(Name = "Asunto")]
        [StringLength(200, ErrorMessage = "El asunto no puede exceder los 200 caracteres")]
        public string Asunto { get; set; } = string.Empty;

        [Required(ErrorMessage = "El mensaje es obligatorio")]
        [Display(Name = "Mensaje")]
        [StringLength(1000, ErrorMessage = "El mensaje no puede exceder los 1000 caracteres")]
        public string Mensaje { get; set; } = string.Empty;
    }
}