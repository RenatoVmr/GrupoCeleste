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
    // Implementa paginación simple mediante 'page' (1-based) y muestra 12 películas por página
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 12;

        var query = _context.Peliculas.AsQueryable();

        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        if (page < 1) page = 1;
        if (page > totalPages && totalPages > 0) page = totalPages;

        var peliculas = await query
            .OrderByDescending(p => p.Calificacion)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

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
