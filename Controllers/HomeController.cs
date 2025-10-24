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
            // Aquí se puede implementar el envío del email
            // Por ahora, solo mostramos un mensaje de éxito
            TempData["MensajeExito"] = "¡Gracias por contactarnos! Hemos recibido tu mensaje y te responderemos pronto.";
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
