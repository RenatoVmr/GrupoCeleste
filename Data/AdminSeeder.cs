using GrupoCeleste.Data;
using GrupoCeleste.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GrupoCeleste.Data
{
    public static class AdminSeeder
    {
        public static async Task CreateAdminUser(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Asegurar que la base de datos esté creada
            await context.Database.EnsureCreatedAsync();

            // Verificar si ya existe el usuario administrador
            var adminUser = await userManager.FindByEmailAsync("admin@cineverse.com");
            
            if (adminUser == null)
            {
                // Crear usuario administrador
                adminUser = new Usuario
                {
                    UserName = "admin@cineverse.com",
                    Email = "admin@cineverse.com",
                    Nombre = "Administrador",
                    Apellido = "CineVerse",
                    EmailConfirmed = true,
                    FechaRegistro = DateTime.Now
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                
                if (result.Succeeded)
                {
                    Console.WriteLine("✅ Usuario administrador creado exitosamente:");
                    Console.WriteLine("📧 Email: admin@cineverse.com");
                    Console.WriteLine("🔐 Contraseña: Admin123!");
                }
                else
                {
                    Console.WriteLine("❌ Error al crear usuario administrador:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"   - {error.Description}");
                    }
                }
            }
            else
            {
                Console.WriteLine("ℹ️  Usuario administrador ya existe:");
                Console.WriteLine("📧 Email: admin@cineverse.com");
                Console.WriteLine("🔐 Contraseña: Admin123!");
            }
        }
    }
}