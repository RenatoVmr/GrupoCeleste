using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GrupoCeleste.Data;
using GrupoCeleste.Models;
using GrupoCeleste.Models.ViewModels;

namespace GrupoCeleste.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PeliculasController> _logger;

        public PeliculasController(ApplicationDbContext context, ILogger<PeliculasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index(
            string? busquedaTitulo,
            string? generoSeleccionado,
            string? directorSeleccionado,
            int? anioDesde,
            int? anioHasta,
            decimal? calificacionMinima,
            string ordenarPor = "Titulo",
            bool ordenDescendente = false,
            int pagina = 1,
            int peliculasPorPagina = 12)
        {
            var query = _context.Peliculas.Where(p => p.EsVisible);

            // Aplicar filtros
            if (!string.IsNullOrEmpty(busquedaTitulo))
            {
                query = query.Where(p => p.Titulo.Contains(busquedaTitulo) || 
                                        p.Descripcion.Contains(busquedaTitulo));
            }

            if (!string.IsNullOrEmpty(generoSeleccionado))
            {
                query = query.Where(p => p.Genero == generoSeleccionado);
            }

            if (!string.IsNullOrEmpty(directorSeleccionado))
            {
                query = query.Where(p => p.Director == directorSeleccionado);
            }

            if (anioDesde.HasValue)
            {
                query = query.Where(p => p.AnioLanzamiento >= anioDesde.Value);
            }

            if (anioHasta.HasValue)
            {
                query = query.Where(p => p.AnioLanzamiento <= anioHasta.Value);
            }

            if (calificacionMinima.HasValue)
            {
                query = query.Where(p => p.Calificacion >= calificacionMinima.Value);
            }

            // Aplicar ordenamiento
            query = ordenarPor.ToLower() switch
            {
                "titulo" => ordenDescendente ? query.OrderByDescending(p => p.Titulo) : query.OrderBy(p => p.Titulo),
                "anio" => ordenDescendente ? query.OrderByDescending(p => p.AnioLanzamiento) : query.OrderBy(p => p.AnioLanzamiento),
                "calificacion" => ordenDescendente ? query.OrderByDescending(p => p.Calificacion) : query.OrderBy(p => p.Calificacion),
                "director" => ordenDescendente ? query.OrderByDescending(p => p.Director) : query.OrderBy(p => p.Director),
                _ => query.OrderBy(p => p.Titulo)
            };

            var totalPeliculas = await query.CountAsync();

            // Aplicar paginación
            var peliculas = await query
                .Skip((pagina - 1) * peliculasPorPagina)
                .Take(peliculasPorPagina)
                .ToListAsync();

            // Obtener listas para filtros
            var generos = await _context.Peliculas
                .Where(p => p.EsVisible)
                .Select(p => p.Genero)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();

            var directores = await _context.Peliculas
                .Where(p => p.EsVisible)
                .Select(p => p.Director)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();

            var viewModel = new PeliculasViewModel
            {
                Peliculas = peliculas,
                BusquedaTitulo = busquedaTitulo,
                GeneroSeleccionado = generoSeleccionado,
                DirectorSeleccionado = directorSeleccionado,
                AnioDesde = anioDesde,
                AnioHasta = anioHasta,
                CalificacionMinima = calificacionMinima,
                OrdenarPor = ordenarPor,
                OrdenDescendente = ordenDescendente,
                PaginaActual = pagina,
                PeliculasPorPagina = peliculasPorPagina,
                TotalPeliculas = totalPeliculas,
                GenerosDisponibles = generos,
                DirectoresDisponibles = directores
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .FirstOrDefaultAsync(m => m.Id == id && m.EsVisible);

            if (pelicula == null)
            {
                return NotFound();
            }

            // Obtener películas similares (mismo género, excluyendo la actual)
            var peliculasSimilares = await _context.Peliculas
                .Where(p => p.Genero == pelicula.Genero && p.Id != pelicula.Id && p.EsVisible)
                .Take(6)
                .ToListAsync();

            var viewModel = new DetallePeliculaViewModel
            {
                Pelicula = pelicula,
                PeliculasSimilares = peliculasSimilares
            };

            return View(viewModel);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pelicula pelicula)
        {
            if (ModelState.IsValid)
            {
                pelicula.FechaCreacion = DateTime.Now;
                pelicula.EsVisible = true;
                
                _context.Add(pelicula);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Película creada: {Titulo}", pelicula.Titulo);
                return RedirectToAction(nameof(Index));
            }
            return View(pelicula);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null || !pelicula.EsVisible)
            {
                return NotFound();
            }
            return View(pelicula);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pelicula pelicula)
        {
            if (id != pelicula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var peliculaExistente = await _context.Peliculas.FindAsync(id);
                    if (peliculaExistente == null)
                    {
                        return NotFound();
                    }

                    // Actualizar propiedades (preservando FechaCreacion)
                    peliculaExistente.Titulo = pelicula.Titulo;
                    peliculaExistente.Descripcion = pelicula.Descripcion;
                    peliculaExistente.Director = pelicula.Director;
                    peliculaExistente.Genero = pelicula.Genero;
                    peliculaExistente.AnioLanzamiento = pelicula.AnioLanzamiento;
                    peliculaExistente.DuracionMinutos = pelicula.DuracionMinutos;
                    peliculaExistente.Calificacion = pelicula.Calificacion;
                    peliculaExistente.ImagenUrl = pelicula.ImagenUrl;
                    peliculaExistente.TrailerUrl = pelicula.TrailerUrl;
                    peliculaExistente.Actores = pelicula.Actores;

                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Película actualizada: {Titulo}", pelicula.Titulo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pelicula);
        }

        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .FirstOrDefaultAsync(m => m.Id == id && m.EsVisible);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula != null)
            {
                // Borrado lógico
                pelicula.EsVisible = false;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Película eliminada (borrado lógico): {Titulo}", pelicula.Titulo);
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaExists(int id)
        {
            return _context.Peliculas.Any(e => e.Id == id && e.EsVisible);
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(string termino)
        {
            if (string.IsNullOrEmpty(termino))
            {
                return Json(new List<object>());
            }

            var peliculas = await _context.Peliculas
                .Where(p => p.EsVisible && (p.Titulo.Contains(termino) || p.Director.Contains(termino)))
                .Select(p => new { 
                    id = p.Id, 
                    titulo = p.Titulo, 
                    director = p.Director,
                    anio = p.AnioLanzamiento 
                })
                .Take(10)
                .ToListAsync();

            return Json(peliculas);
        }
    }
}