using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GrupoCeleste.Data;
using GrupoCeleste.Models;

namespace GrupoCeleste.Controllers
{
    [Authorize] // Solo usuarios autenticados pueden acceder
    public class MensajesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MensajesController> _logger;

        public MensajesController(ApplicationDbContext context, ILogger<MensajesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Mensajes
        public async Task<IActionResult> Index()
        {
            var mensajes = await _context.Mensajes
                .OrderByDescending(m => m.Fecha)
                .ToListAsync();

            ViewBag.TotalMensajes = mensajes.Count;
            ViewBag.MensajesNoLeidos = mensajes.Count(m => !m.Leido);

            return View(mensajes);
        }

        // GET: Mensajes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mensaje = await _context.Mensajes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (mensaje == null)
            {
                return NotFound();
            }

            // Marcar como leído si no lo estaba
            if (!mensaje.Leido)
            {
                mensaje.Leido = true;
                mensaje.FechaLectura = DateTime.Now;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Mensaje {MensajeId} marcado como leído", mensaje.Id);
            }

            return View(mensaje);
        }

        // POST: Mensajes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mensaje = await _context.Mensajes.FindAsync(id);
            if (mensaje != null)
            {
                _context.Mensajes.Remove(mensaje);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation("Mensaje {MensajeId} eliminado", mensaje.Id);
                TempData["MensajeExito"] = "Mensaje eliminado correctamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Mensajes/MarkAsRead/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var mensaje = await _context.Mensajes.FindAsync(id);
            if (mensaje != null)
            {
                mensaje.Leido = true;
                mensaje.FechaLectura = DateTime.Now;
                await _context.SaveChangesAsync();

                _logger.LogInformation("Mensaje {MensajeId} marcado como leído", mensaje.Id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}