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

    public async Task<IActionResult> Index(string? genero = null)
    {
        var query = _context.Peliculas.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(genero))
        {
            query = query.Where(p => p.Genero.ToLower() == genero.ToLower());
        }

        var peliculas = await query.ToListAsync();
        
        // Lista de géneros para los botones
        ViewBag.Generos = new[] { "Acción", "Drama", "Comedia", "Terror", "Romance", "Sci-Fi" };
        ViewBag.GeneroSeleccionado = genero;
        
        return View(peliculas);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Contacto()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Contacto(ContactoViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Crear el mensaje y guardarlo en la base de datos
            var mensaje = new Mensaje
            {
                Nombre = model.Nombre,
                Email = model.Email,
                Asunto = model.Asunto,
                Contenido = model.Mensaje,
                FechaEnvio = DateTime.Now,
                Leido = false
            };

            _context.Mensajes.Add(mensaje);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "¡Gracias por contactarnos! Tu mensaje ha sido guardado y lo revisaremos pronto.";
            return RedirectToAction("Contacto");
        }

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
