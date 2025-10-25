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
    // Soporta búsqueda por título o director mediante 'search' (query string)
    public async Task<IActionResult> Index(string? search)
    {
        var query = _context.Peliculas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            query = query.Where(p => p.Titulo.Contains(s) || p.Director.Contains(s));
        }

        var peliculas = await query
            .OrderByDescending(p => p.Calificacion)
            .ToListAsync();

        ViewBag.SearchTerm = search ?? string.Empty;
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
