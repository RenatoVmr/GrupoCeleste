using GrupoCeleste.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GrupoCeleste.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new ApplicationDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());

        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<Usuario>>();

        // Crear roles
        await CreateRoles(roleManager);
        
        // Crear usuario administrador por defecto
        await CreateAdminUser(userManager);

        // Crear películas de ejemplo si no existen
        if (context.Peliculas.Any())
        {
            return; // La base de datos ya tiene datos
        }

        context.Peliculas.AddRange(
            new Pelicula
            {
                Titulo = "El Padrino",
                Descripcion = "La historia de la familia Corleone, una de las familias más poderosas de la mafia italiana en Nueva York.",
                Director = "Francis Ford Coppola",
                Genero = "Drama",
                AnioLanzamiento = 1972,
                DuracionMinutos = 175,
                Calificacion = 9.2,
                Actores = "Marlon Brando, Al Pacino, James Caan",
                ImagenUrl = "https://image.tmdb.org/t/p/w500/3bhkrj58Vtu7enYsRolD1fZdja1.jpg"
            },
            new Pelicula
            {
                Titulo = "El Origen",
                Descripcion = "Un ladrón que roba secretos corporativos a través del uso de la tecnología de compartir sueños.",
                Director = "Christopher Nolan",
                Genero = "Sci-Fi",
                AnioLanzamiento = 2010,
                DuracionMinutos = 148,
                Calificacion = 8.8,
                Actores = "Leonardo DiCaprio, Joseph Gordon-Levitt, Ellen Page",
                ImagenUrl = "https://image.tmdb.org/t/p/w500/9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg"
            },
            new Pelicula
            {
                Titulo = "Avengers: Endgame",
                Descripcion = "Los Vengadores restantes deben encontrar una manera de recuperar a sus aliados para un enfrentamiento épico con Thanos.",
                Director = "Anthony y Joe Russo",
                Genero = "Acción",
                AnioLanzamiento = 2019,
                DuracionMinutos = 181,
                Calificacion = 8.4,
                Actores = "Robert Downey Jr., Chris Evans, Scarlett Johansson",
                ImagenUrl = "https://image.tmdb.org/t/p/w500/or06FN3Dka5tukK1e9sl16pB3iy.jpg"
            },
            new Pelicula
            {
                Titulo = "Titanic",
                Descripcion = "Una historia de amor épica entre Jack y Rose en el viaje inaugural del RMS Titanic.",
                Director = "James Cameron",
                Genero = "Romance",
                AnioLanzamiento = 1997,
                DuracionMinutos = 194,
                Calificacion = 7.9,
                Actores = "Leonardo DiCaprio, Kate Winslet, Billy Zane",
                ImagenUrl = "https://image.tmdb.org/t/p/w500/9xjZS2rlVxm8SFx8kPC3aIGCOYQ.jpg"
            },
            new Pelicula
            {
                Titulo = "El Caballero de la Noche",
                Descripcion = "Batman debe aceptar una de las pruebas psicológicas y físicas más grandes de su capacidad para luchar contra la injusticia.",
                Director = "Christopher Nolan",
                Genero = "Acción",
                AnioLanzamiento = 2008,
                DuracionMinutos = 152,
                Calificacion = 9.0,
                Actores = "Christian Bale, Heath Ledger, Aaron Eckhart",
                ImagenUrl = "https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg"
            },
            new Pelicula
            {
                Titulo = "Interstellar",
                Descripcion = "Un equipo de exploradores viaja a través de un agujero de gusano en el espacio en un intento de garantizar la supervivencia de la humanidad.",
                Director = "Christopher Nolan",
                Genero = "Sci-Fi",
                AnioLanzamiento = 2014,
                DuracionMinutos = 169,
                Calificacion = 8.6,
                Actores = "Matthew McConaughey, Anne Hathaway, Jessica Chastain",
                ImagenUrl = "https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg"
            }
        );

        await context.SaveChangesAsync();
    }

    private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
    {
        // Crear rol de Admin si no existe
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }

        // Crear rol de Usuario si no existe
        if (!await roleManager.RoleExistsAsync("Usuario"))
        {
            await roleManager.CreateAsync(new IdentityRole("Usuario"));
        }
    }

    private static async Task CreateAdminUser(UserManager<Usuario> userManager)
    {
        // Verificar si ya existe un usuario administrador
        var adminUser = await userManager.FindByEmailAsync("admin@grupoceleste.com");
        
        if (adminUser == null)
        {
            // Crear usuario administrador por defecto
            adminUser = new Usuario
            {
                UserName = "admin@grupoceleste.com",
                Email = "admin@grupoceleste.com",
                Nombre = "Administrador",
                Apellido = "Sistema",
                EmailConfirmed = true,
                FechaRegistro = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            
            if (result.Succeeded)
            {
                // Asignar rol de administrador
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        else
        {
            // Asegurar que tenga el rol de admin si ya existe el usuario
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
