using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GrupoCeleste.Data;
using GrupoCeleste.Models;
using Microsoft.EntityFrameworkCore;

namespace GrupoCeleste.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Panel principal del administrador
    public async Task<IActionResult> Index()
    {
        var totalPeliculas = await _context.Peliculas.CountAsync();
        ViewBag.TotalPeliculas = totalPeliculas;
        
        var peliculasRecientes = await _context.Peliculas
            .OrderByDescending(p => p.FechaCreacion)
            .Take(12)
            .ToListAsync();
        
        return View(peliculasRecientes);
    }

    // GET: Admin/CrearPelicula
    public IActionResult CrearPelicula()
    {
        return View();
    }

    // POST: Admin/CrearPelicula
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CrearPelicula(Pelicula pelicula)
    {
        if (ModelState.IsValid)
        {
            try
            {
                pelicula.FechaCreacion = DateTime.UtcNow;
                _context.Peliculas.Add(pelicula);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Película creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al crear la película: " + ex.Message);
            }
        }

        return View(pelicula);
    }

    // GET: Admin/EditarPelicula/5
    public async Task<IActionResult> EditarPelicula(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var pelicula = await _context.Peliculas.FindAsync(id);
        if (pelicula == null)
        {
            return NotFound();
        }
        
        return View(pelicula);
    }

    // POST: Admin/EditarPelicula/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditarPelicula(int id, Pelicula pelicula)
    {
        if (id != pelicula.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(pelicula);
                await _context.SaveChangesAsync();
                
                TempData["SuccessMessage"] = "Película actualizada exitosamente.";
                return RedirectToAction(nameof(Index));
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
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al actualizar la película: " + ex.Message);
            }
        }
        
        return View(pelicula);
    }

    // POST: Admin/EliminarPelicula/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarPelicula(int id)
    {
        try
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula != null)
            {
                _context.Peliculas.Remove(pelicula);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Película eliminada exitosamente.";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "Error al eliminar la película: " + ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // GET: Admin/TodasLasPeliculas
    public async Task<IActionResult> TodasLasPeliculas()
    {
        var todasLasPeliculas = await _context.Peliculas
            .OrderByDescending(p => p.FechaCreacion)
            .ToListAsync();
        
        ViewBag.TotalPeliculas = todasLasPeliculas.Count;
        ViewData["Title"] = "Gestión de Películas";
        
        return View(todasLasPeliculas);
    }

    private bool PeliculaExists(int id)
    {
        return _context.Peliculas.Any(e => e.Id == id);
    }
}