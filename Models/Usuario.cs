using Microsoft.AspNetCore.Identity;

namespace GrupoCeleste.Models;

public class Usuario : IdentityUser
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
}
