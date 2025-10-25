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

            // Crear usuario administrador
            await CreateUserIfNotExists(userManager, "admin@cineverse.com", "Admin123!", 
                "Administrador", "CineVerse", isAdmin: true);

            // Crear usuarios de prueba
            await CreateTestUsers(userManager);
        }

        private static async Task CreateUserIfNotExists(UserManager<Usuario> userManager, 
            string email, string password, string nombre, string apellido, bool isAdmin = false)
        {
            var user = await userManager.FindByEmailAsync(email);
            
            if (user == null)
            {
                user = new Usuario
                {
                    UserName = email,
                    Email = email,
                    Nombre = nombre,
                    Apellido = apellido,
                    EmailConfirmed = true,
                    FechaRegistro = DateTime.Now
                };

                var result = await userManager.CreateAsync(user, password);
                
                if (result.Succeeded)
                {
                    if (isAdmin)
                    {
                        Console.WriteLine("✅ Usuario administrador creado exitosamente:");
                    }
                    else
                    {
                        Console.WriteLine($"✅ Usuario de prueba creado: {nombre} {apellido}");
                    }
                    Console.WriteLine($"📧 Email: {email}");
                    Console.WriteLine($"🔐 Contraseña: {password}");
                }
                else
                {
                    Console.WriteLine($"❌ Error al crear usuario {email}:");
                    foreach (var error in result.Errors)
                    {
                        Console.WriteLine($"   - {error.Description}");
                    }
                }
            }
            else
            {
                if (isAdmin)
                {
                    Console.WriteLine("ℹ️  Usuario administrador ya existe:");
                    Console.WriteLine($"📧 Email: {email}");
                    Console.WriteLine("🔐 Contraseña: Admin123!");
                }
            }
        }

        private static async Task CreateTestUsers(UserManager<Usuario> userManager)
        {
            Console.WriteLine("\n🧪 Creando usuarios de prueba...");
            
            // Usuario de prueba 1
            await CreateUserIfNotExists(userManager, "user1@test.com", "Test123!", 
                "Juan", "Pérez");

            // Usuario de prueba 2
            await CreateUserIfNotExists(userManager, "user2@test.com", "Test123!", 
                "María", "García");

            // Usuario de prueba 3
            await CreateUserIfNotExists(userManager, "user3@test.com", "Test123!", 
                "Carlos", "López");

            // Usuario de prueba 4
            await CreateUserIfNotExists(userManager, "test@cineverse.com", "Prueba123!", 
                "Usuario", "Prueba");

            Console.WriteLine("🎉 Usuarios de prueba listos para usar!\n");
        }
    }
}