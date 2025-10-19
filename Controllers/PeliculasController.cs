using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrupoCeleste.Data;
using GrupoCeleste.Models;

namespace GrupoCeleste.Controllers;

public class PeliculasController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PeliculasController> _logger;

    public PeliculasController(ApplicationDbContext context, ILogger<PeliculasController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /Peliculas
    public async Task<IActionResult> Index()
    {
        var peliculas = await _context.Peliculas
            .OrderByDescending(p => p.Calificacion)
            .ToListAsync();

        return View(peliculas);
    }

    // GET: /Peliculas/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pelicula = await _context.Peliculas
            .FirstOrDefaultAsync(m => m.Id == id);

        if (pelicula == null)
        {
            return NotFound();
        }

        return View(pelicula);
    }
}
