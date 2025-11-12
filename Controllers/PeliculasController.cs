using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GrupoCeleste.Data;
using GrupoCeleste.Models;
using GrupoCeleste.ViewModels;

namespace GrupoCeleste.Controllers;

public class PeliculasController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PeliculasController> _logger;
    private readonly UserManager<Usuario> _userManager;

    public PeliculasController(ApplicationDbContext context, ILogger<PeliculasController> logger, UserManager<Usuario> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
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

        // Obtener reseñas de la película
        var resenas = await _context.Resenas
            .Include(r => r.Usuario)
            .Where(r => r.PeliculaId == id)
            .OrderByDescending(r => r.FechaCreacion)
            .Select(r => new ResenaViewModel
            {
                Id = r.Id,
                Calificacion = r.Calificacion,
                Comentario = r.Comentario,
                FechaCreacion = r.FechaCreacion,
                NombreUsuario = r.Usuario != null ? $"{r.Usuario.Nombre} {r.Usuario.Apellido}" : "Usuario anónimo"
            })
            .ToListAsync();

        // Calcular promedio de calificaciones
        double promedioCalificaciones = 0;
        if (resenas.Any())
        {
            promedioCalificaciones = Math.Round(resenas.Average(r => r.Calificacion), 1);
        }

        // Verificar si el usuario está autenticado y puede reseñar
        bool usuarioPuedeResenar = User.Identity?.IsAuthenticated == true;
        bool usuarioYaReseno = false;

        if (usuarioPuedeResenar)
        {
            var userId = _userManager.GetUserId(User);
            usuarioYaReseno = await _context.Resenas
                .AnyAsync(r => r.PeliculaId == id && r.UsuarioId == userId);
        }

        var viewModel = new PeliculaDetailsViewModel
        {
            Pelicula = pelicula,
            Resenas = resenas,
            PromedioCalificaciones = promedioCalificaciones,
            TotalResenas = resenas.Count,
            UsuarioPuedeResenar = usuarioPuedeResenar && !usuarioYaReseno,
            UsuarioYaReseno = usuarioYaReseno,
            NuevaResena = usuarioPuedeResenar && !usuarioYaReseno ? new NuevaResenaViewModel { PeliculaId = pelicula.Id } : null
        };

        return View(viewModel);
    }

    // POST: /Peliculas/CrearResena
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CrearResena(int PeliculaId, int Calificacion, string Comentario)
    {
        _logger.LogInformation("CrearResena called with PeliculaId: {PeliculaId}, Calificacion: {Calificacion}, Comentario: {Comentario}", 
            PeliculaId, Calificacion, Comentario);

        var userId = _userManager.GetUserId(User);
        if (string.IsNullOrEmpty(userId))
        {
            TempData["Error"] = "Debes iniciar sesión para escribir una reseña.";
            return RedirectToAction(nameof(Details), new { id = PeliculaId });
        }

        // Validaciones básicas
        if (Calificacion < 1 || Calificacion > 5)
        {
            TempData["Error"] = "La calificación debe estar entre 1 y 5 estrellas.";
            return RedirectToAction(nameof(Details), new { id = PeliculaId });
        }

        if (string.IsNullOrWhiteSpace(Comentario) || Comentario.Length < 10)
        {
            TempData["Error"] = "El comentario debe tener al menos 10 caracteres.";
            return RedirectToAction(nameof(Details), new { id = PeliculaId });
        }

        // Verificar que la película existe
        var peliculaExiste = await _context.Peliculas.AnyAsync(p => p.Id == PeliculaId);
        if (!peliculaExiste)
        {
            TempData["Error"] = "La película no existe.";
            return RedirectToAction(nameof(Index));
        }

        // Verificar que el usuario no haya reseñado ya esta película
        var yaReseno = await _context.Resenas
            .AnyAsync(r => r.PeliculaId == PeliculaId && r.UsuarioId == userId);

        if (yaReseno)
        {
            TempData["Error"] = "Ya has escrito una reseña para esta película.";
            return RedirectToAction(nameof(Details), new { id = PeliculaId });
        }

        try
        {
            var resena = new Resena
            {
                PeliculaId = PeliculaId,
                UsuarioId = userId,
                Calificacion = Calificacion,
                Comentario = Comentario.Trim(),
                FechaCreacion = DateTime.UtcNow
            };

            _context.Resenas.Add(resena);
            await _context.SaveChangesAsync();

            TempData["Success"] = "¡Tu reseña ha sido publicada exitosamente!";
            _logger.LogInformation("Usuario {UserId} creó una reseña para la película {PeliculaId}", userId, PeliculaId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear reseña para la película {PeliculaId} por el usuario {UserId}", PeliculaId, userId);
            TempData["Error"] = "Ocurrió un error al publicar tu reseña. Inténtalo de nuevo.";
        }

        return RedirectToAction(nameof(Details), new { id = PeliculaId });
    }
}
