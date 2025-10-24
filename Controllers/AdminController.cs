using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GrupoCeleste.Data;
using GrupoCeleste.Models;

namespace GrupoCeleste.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Mensajes()
        {
            var mensajes = await _context.Mensajes
                .OrderByDescending(m => m.FechaEnvio)
                .ToListAsync();
            
            return View(mensajes);
        }

        public async Task<IActionResult> MarcarLeido(int id)
        {
            var mensaje = await _context.Mensajes.FindAsync(id);
            if (mensaje != null)
            {
                mensaje.Leido = true;
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Mensajes");
        }

        public async Task<IActionResult> EliminarMensaje(int id)
        {
            var mensaje = await _context.Mensajes.FindAsync(id);
            if (mensaje != null)
            {
                _context.Mensajes.Remove(mensaje);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction("Mensajes");
        }
    }
}