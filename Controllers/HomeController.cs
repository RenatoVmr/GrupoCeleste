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
    public IActionResult Contacto(ContactoViewModel model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Simular procesamiento del formulario
                _logger.LogInformation("Nuevo mensaje de contacto recibido de: {Email}, Asunto: {Asunto}", 
                    model.Email, model.Asunto);
                
                // En un futuro aquí se puede implementar:
                // - Envío de email
                // - Guardar en base de datos
                // - Integración con sistema de tickets
                
                TempData["MensajeExito"] = $"¡Gracias {model.Nombre}! Hemos recibido tu mensaje sobre '{model.Asunto}' y te responderemos a {model.Email} pronto.";
                
                // Limpiar el formulario después del envío exitoso
                return RedirectToAction("Contacto");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar el formulario de contacto");
                TempData["MensajeError"] = "Ocurrió un error al enviar tu mensaje. Por favor, inténtalo nuevamente.";
            }
        }
        
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
