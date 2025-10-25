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
    // Añadido parámetro opcional 'genre' para filtrar por género.
    public async Task<IActionResult> Index(string? genre)
    {
        var query = _context.Peliculas.AsQueryable();

        if (!string.IsNullOrEmpty(genre))
        {
            query = query.Where(p => p.Genero == genre);
        }

        var peliculas = await query
            .OrderByDescending(p => p.Calificacion)
            .ToListAsync();

        // Lista de géneros disponibles (botones/menú)
        var genres = new List<string> { "Acción", "Drama", "Comedia", "Terror", "Romance", "Sci-Fi" };
        ViewBag.Genres = genres;
        ViewBag.SelectedGenre = genre ?? string.Empty;

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
