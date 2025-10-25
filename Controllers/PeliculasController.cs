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
    // Soporta búsqueda por título/director, filtrado por género y paginación
    public async Task<IActionResult> Index(string? search, string? genre, int page = 1)
    {
        const int pageSize = 12;

        var query = _context.Peliculas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim();
            // búsqueda case-insensitive y parcial
            query = query.Where(p => EF.Functions.Like(p.Titulo.ToLower(), $"%{s.ToLower()}%")
                                     || EF.Functions.Like(p.Director.ToLower(), $"%{s.ToLower()}%"));
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            query = query.Where(p => p.Genero.ToLower() == genre.ToLower());
        }

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        if (page < 1) page = 1;
        if (page > totalPages && totalPages > 0) page = totalPages;

        var peliculas = await query
            .OrderByDescending(p => p.Calificacion)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Datos para la vista
        ViewBag.SearchTerm = search ?? string.Empty;
        ViewBag.Genres = new List<string> { "Acción", "Drama", "Comedia", "Terror", "Romance", "Sci-Fi" };
        ViewBag.SelectedGenre = genre ?? string.Empty;
        ViewBag.Page = page;
        ViewBag.TotalPages = totalPages;
        ViewBag.PageSize = pageSize;

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
