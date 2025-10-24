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
        return View();
    }

    [HttpPost]
    public IActionResult Contacto(ContactoViewModel model)
    {
        if (ModelState.IsValid)
        {
            // Aquí se procesaría el formulario (por ahora solo simularemos el envío)
            TempData["Mensaje"] = "¡Gracias por contactarnos! Tu mensaje ha sido enviado exitosamente.";
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
