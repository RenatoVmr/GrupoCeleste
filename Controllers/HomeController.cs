using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GrupoCeleste.Models;
using GrupoCeleste.Data;
using Microsoft.EntityFrameworkCore;

namespace GrupoCeleste.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var peliculas = await _context.Peliculas.ToListAsync();
        return View(peliculas);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Contacto()
    {
        return View(new ContactoViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Contacto(ContactoViewModel model)
    {
        // Validación adicional de campos obligatorios
        if (string.IsNullOrWhiteSpace(model.Nombre))
        {
            ModelState.AddModelError(nameof(model.Nombre), "El nombre es obligatorio");
        }
        
        if (string.IsNullOrWhiteSpace(model.Email))
        {
            ModelState.AddModelError(nameof(model.Email), "El email es obligatorio");
        }
        
        if (string.IsNullOrWhiteSpace(model.Asunto))
        {
            ModelState.AddModelError(nameof(model.Asunto), "El asunto es obligatorio");
        }
        
        if (string.IsNullOrWhiteSpace(model.Mensaje))
        {
            ModelState.AddModelError(nameof(model.Mensaje), "El mensaje es obligatorio");
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Log del mensaje recibido
                _logger.LogInformation("Formulario de contacto procesado exitosamente. Usuario: {Nombre}, Email: {Email}, Asunto: {Asunto}", 
                    model.Nombre, model.Email, model.Asunto);
                
                // Validación adicional de formato de email
                if (!IsValidEmail(model.Email))
                {
                    ModelState.AddModelError(nameof(model.Email), "El formato del email no es válido");
                    return View(model);
                }
                
                // Guardar mensaje en base de datos
                var mensaje = new Mensaje
                {
                    Nombre = model.Nombre,
                    Email = model.Email,
                    Asunto = model.Asunto,
                    MensajeTexto = model.Mensaje,
                    Telefono = model.Telefono,
                    Fecha = DateTime.Now,
                    Leido = false
                };

                _context.Mensajes.Add(mensaje);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Mensaje guardado en base de datos con ID: {MensajeId}", mensaje.Id);
                
                // Mensaje de confirmación detallado tras envío exitoso
                TempData["MensajeExito"] = $"✅ ¡Mensaje enviado exitosamente!\n\n" +
                    $"Estimado/a {model.Nombre},\n\n" +
                    $"Hemos recibido tu consulta sobre: '{model.Asunto}'\n" +
                    $"Te enviaremos una respuesta a: {model.Email}\n" +
                    $"Tiempo estimado de respuesta: 24-48 horas.\n\n" +
                    $"¡Gracias por contactar con CineVerse!";
                
                // Limpiar el formulario después del envío exitoso
                return RedirectToAction("Contacto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el formulario de contacto del usuario {Email}", model.Email);
                TempData["MensajeError"] = "❌ Error: No pudimos procesar tu mensaje. Por favor, verifica todos los campos e inténtalo nuevamente. Si el problema persiste, contáctanos directamente.";
            }
        }
        else
        {
            // Log de errores de validación
            _logger.LogWarning("Formulario de contacto con errores de validación. Errores: {Errores}", 
                string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            
            TempData["MensajeError"] = "⚠️ Por favor, corrige los errores en el formulario antes de enviarlo.";
        }
        
        return View(model);
    }
    
    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
