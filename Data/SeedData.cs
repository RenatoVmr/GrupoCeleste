using GrupoCeleste.Data;
using GrupoCeleste.Models;
using Microsoft.AspNetCore.Identity;

namespace GrupoCeleste.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<Usuario> userManager, ApplicationDbContext context)
        {
            // Asegurar que la base de datos existe
            await context.Database.EnsureCreatedAsync();

            // Verificar si ya hay películas
            if (context.Peliculas.Any())
            {
                return; // La base de datos ya está poblada
            }

            // Crear usuario administrador si no existe
            var adminUser = await userManager.FindByEmailAsync("admin@grupoceleste.com");
            if (adminUser == null)
            {
                adminUser = new Usuario
                {
                    UserName = "admin@grupoceleste.com",
                    Email = "admin@grupoceleste.com",
                    Nombre = "Administrador",
                    Apellido = "Sistema",
                    EmailConfirmed = true,
                    EsActivo = true
                };
                await userManager.CreateAsync(adminUser, "Admin123!");
            }

            // Crear datos de ejemplo de películas
            var peliculas = new List<Pelicula>
            {
                new Pelicula
                {
                    Titulo = "El Padrino",
                    Descripcion = "La historia de la familia Corleone, una poderosa familia de la mafia italiana en Nueva York. Cuando el patriarca Vito Corleone es atacado, su hijo menor Michael se ve obligado a entrar en el negocio familiar.",
                    Director = "Francis Ford Coppola",
                    Genero = "Drama",
                    AnioLanzamiento = 1972,
                    DuracionMinutos = 175,
                    Calificacion = 9.2m,
                    Actores = "Marlon Brando, Al Pacino, James Caan",
                    ImagenUrl = "https://image.tmdb.org/t/p/w500/3bhkrj58Vtu7enYsRolD1fZdja1.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "Pulp Fiction",
                    Descripcion = "Historias entrelazadas de crimen en Los Ángeles. Dos asesinos a sueldo, un boxeador y la esposa de un gánster se ven involucrados en una serie de eventos violentos y surrealistas.",
                    Director = "Quentin Tarantino",
                    Genero = "Suspense",
                    AnioLanzamiento = 1994,
                    DuracionMinutos = 154,
                    Calificacion = 8.9m,
                    Actores = "John Travolta, Uma Thurman, Samuel L. Jackson",
                    ImagenUrl = "https://image.tmdb.org/t/p/w500/d5iIlFn5s0ImszYzBPb8JPIfbXD.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "El Señor de los Anillos: El Retorno del Rey",
                    Descripcion = "La batalla final por la Tierra Media comienza. Aragorn debe asumir su destino como rey, mientras Frodo y Sam se acercan al Monte del Destino para destruir el Anillo Único.",
                    Director = "Peter Jackson",
                    Genero = "Fantasía",
                    AnioLanzamiento = 2003,
                    DuracionMinutos = 201,
                    Calificacion = 8.9m,
                    Actores = "Elijah Wood, Viggo Mortensen, Ian McKellen",
                    ImagenUrl = "https://image.tmdb.org/t/p/w500/rCzpDGLbOoPwLjy3OAm5NUPOTrC.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "Forrest Gump",
                    Descripcion = "La extraordinaria vida de Forrest Gump, un hombre con discapacidad intelectual que, sin darse cuenta, influye en varios eventos históricos importantes del siglo XX en Estados Unidos.",
                    Director = "Robert Zemeckis",
                    Genero = "Drama",
                    AnioLanzamiento = 1994,
                    DuracionMinutos = 142,
                    Calificacion = 8.8m,
                    Actores = "Tom Hanks, Robin Wright, Gary Sinise",
                    ImagenUrl = "https://image.tmdb.org/t/p/w500/arw2vcBveWOVZr6pxd9XTd1TdQa.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "El Origen",
                    Descripcion = "Dom Cobb es un ladrón especializado en el arte de la extracción, robando secretos del subconsciente durante el estado de sueño. Ahora debe realizar la tarea inversa: la inception.",
                    Director = "Christopher Nolan",
                    Genero = "Ciencia Ficción",
                    AnioLanzamiento = 2010,
                    DuracionMinutos = 148,
                    Calificacion = 8.7m,
                    Actores = "Leonardo DiCaprio, Marion Cotillard, Tom Hardy",
                    ImagenUrl = "https://image.tmdb.org/t/p/w500/9gk7adHYeDvHkCSEqAvQNLV5Uge.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "Avengers: Endgame",
                    Descripcion = "Después de los devastadores eventos de Infinity War, los Vengadores restantes se unen una vez más para deshacer las acciones de Thanos y restaurar el orden en el universo.",
                    Director = "Anthony Russo, Joe Russo",
                    Genero = "Acción",
                    AnioLanzamiento = 2019,
                    DuracionMinutos = 181,
                    Calificacion = 8.4m,
                    Actores = "Robert Downey Jr., Chris Evans, Scarlett Johansson",
                    ImagenUrl = "https://image.tmdb.org/t/p/w500/or06FN3Dka5tukK1e9sl16pB3iy.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "Titanic",
                    Descripcion = "Una historia de amor épica entre Jack y Rose a bordo del RMS Titanic, el barco más lujoso de su época que se hundió en su viaje inaugural.",
                    Director = "James Cameron",
                    Genero = "Romance",
                    AnioLanzamiento = 1997,
                    DuracionMinutos = 194,
                    Calificacion = 7.9m,
                    Actores = "Leonardo DiCaprio, Kate Winslet, Billy Zane",
                    ImagenUrl = "https://image.tmdb.org/t/p/w500/9xjZS2rlVxm8SFx8kPC3aIGCOYQ.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "El Caballero de la Noche",
                    Descripcion = "Batman se enfrenta al Joker, un criminal psicópata que busca sumir Gotham City en el caos y poner a prueba el código moral del héroe.",
                    Director = "Christopher Nolan",
                    Genero = "Acción",
                    AnioLanzamiento = 2008,
                    DuracionMinutos = 152,
                    Calificacion = 9.0m,
                    Actores = "Christian Bale, Heath Ledger, Aaron Eckhart",
                    ImagenUrl = "https://image.tmdb.org/t/p/w500/qJ2tW6WMUDux911r6m7haRef0WH.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "Matrix",
                    Descripcion = "Un programador descubre que la realidad tal como la conoce no es más que una simulación y debe elegir entre la ignorancia cómoda o la dura verdad.",
                    Director = "Lana Wachowski, Lilly Wachowski",
                    Genero = "Ciencia Ficción",
                    AnioLanzamiento = 1999,
                    DuracionMinutos = 136,
                    Calificacion = 8.7m,
                    Actores = "Keanu Reeves, Laurence Fishburne, Carrie-Anne Moss",
                    ImagenUrl = "https://image.tmdb.org/t/p/w500/f89U3ADr1oiB1s9GkdPOEpXUk5H.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "Spider-Man: No Way Home",
                    Descripcion = "Peter Parker busca la ayuda del Doctor Strange para hacer que el mundo olvide su identidad secreta, pero el hechizo sale mal y villanos de otros universos invaden su realidad.",
                    Director = "Jon Watts",
                    Genero = "Acción",
                    AnioLanzamiento = 2021,
                    DuracionMinutos = 148,
                    Calificacion = 8.2m,
                    Actores = "Tom Holland, Zendaya, Benedict Cumberbatch",
                    ImagenUrl = "https://image.tmdb.org/t/p/w500/1g0dhYtq4irTY1GPXvft6k4YLjm.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "Interstellar",
                    Descripcion = "En un futuro distópico, un grupo de astronautas viaja a través de un agujero de gusano en busca de un nuevo hogar para la humanidad.",
                    Director = "Christopher Nolan",
                    Genero = "Ciencia Ficción",
                    AnioLanzamiento = 2014,
                    DuracionMinutos = 169,
                    Calificacion = 8.6m,
                    Actores = "Matthew McConaughey, Anne Hathaway, Jessica Chastain",
                    ImagenUrl = "https://image.tmdb.org/t/p/w500/gEU2QniE6E77NI6lCU6MxlNBvIx.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "Dune",
                    Descripcion = "Paul Atreides, un joven brillante y talentoso nacido con un gran destino más allá de su comprensión, debe viajar al planeta más peligroso del universo.",
                    Director = "Denis Villeneuve",
                    Genero = "Ciencia Ficción",
                    AnioLanzamiento = 2021,
                    DuracionMinutos = 155,
                    Calificacion = 8.0m,
                    Actores = "Timothée Chalamet, Rebecca Ferguson, Oscar Isaac",
                    ImagenUrl = "https://image.tmdb.org/t/p/w500/d5NXSklXo0qyIYkgV94XAgMIckC.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "Top Gun: Maverick",
                    Descripcion = "Después de más de 30 años de servicio, Pete 'Maverick' Mitchell sigue siendo uno de los mejores pilotos de la Marina, pero debe enfrentar los fantasmas de su pasado.",
                    Director = "Joseph Kosinski",
                    Genero = "Acción",
                    AnioLanzamiento = 2022,
                    DuracionMinutos = 131,
                    Calificacion = 8.3m,
                    Actores = "Tom Cruise, Miles Teller, Jennifer Connelly",
                    ImagenUrl = "https://image.tmdb.org/t/p/w500/62HCnUTziyWcpDaBO2i1DX17ljH.jpg",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "Toy Story",
                    Descripcion = "Los juguetes de Andy cobran vida cuando él no está. Woody, un vaquero de juguete, se siente amenazado cuando llega Buzz Lightyear, un nuevo juguete espacial.",
                    Director = "John Lasseter",
                    Genero = "Animación",
                    AnioLanzamiento = 1995,
                    DuracionMinutos = 81,
                    Calificacion = 8.3m,
                    Actores = "Tom Hanks, Tim Allen, Don Rickles",
                    ImagenUrl = "https://via.placeholder.com/300x450/FFD700/FFFFFF?text=Toy+Story",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "Titanic",
                    Descripcion = "Una joven aristocrática se enamora de un artista pobre a bordo del lujoso y desafortunado R.M.S. Titanic en su viaje inaugural en 1912.",
                    Director = "James Cameron",
                    Genero = "Romance",
                    AnioLanzamiento = 1997,
                    DuracionMinutos = 195,
                    Calificacion = 7.8m,
                    Actores = "Leonardo DiCaprio, Kate Winslet, Billy Zane",
                    ImagenUrl = "https://via.placeholder.com/300x450/000080/FFFFFF?text=Titanic",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "El Exorcista",
                    Descripcion = "Una niña de 12 años es poseída por una entidad demoníaca misteriosa, forzando a su madre a buscar la ayuda de dos sacerdotes para salvar a su hija.",
                    Director = "William Friedkin",
                    Genero = "Terror",
                    AnioLanzamiento = 1973,
                    DuracionMinutos = 122,
                    Calificacion = 8.0m,
                    Actores = "Ellen Burstyn, Max von Sydow, Linda Blair",
                    ImagenUrl = "https://via.placeholder.com/300x450/800000/FFFFFF?text=El+Exorcista",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "La La Land",
                    Descripcion = "Una pianista de jazz y una actriz en ciernes se enamoran en Los Ángeles mientras persiguen sus sueños artísticos en esta moderna historia de amor musical.",
                    Director = "Damien Chazelle",
                    Genero = "Musical",
                    AnioLanzamiento = 2016,
                    DuracionMinutos = 128,
                    Calificacion = 8.0m,
                    Actores = "Ryan Gosling, Emma Stone, John Legend",
                    ImagenUrl = "https://via.placeholder.com/300x450/9370DB/FFFFFF?text=La+La+Land",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "Coco",
                    Descripcion = "Un niño mexicano que sueña con convertirse en músico debe viajar a la Tierra de los Muertos para descubrir la verdad detrás de la prohibición de su familia hacia la música.",
                    Director = "Lee Unkrich",
                    Genero = "Animación",
                    AnioLanzamiento = 2017,
                    DuracionMinutos = 105,
                    Calificacion = 8.4m,
                    Actores = "Anthony Gonzalez, Gael García Bernal, Benjamin Bratt",
                    ImagenUrl = "https://via.placeholder.com/300x450/FF4500/FFFFFF?text=Coco",
                    FechaCreacion = DateTime.Now
                },
                new Pelicula
                {
                    Titulo = "Mad Max: Fury Road",
                    Descripcion = "En un futuro post-apocalíptico, Max se une a Furiosa para escapar de un señor de la guerra tiránico que controla la tierra y el agua en un desierto desolado.",
                    Director = "George Miller",
                    Genero = "Acción",
                    AnioLanzamiento = 2015,
                    DuracionMinutos = 120,
                    Calificacion = 8.1m,
                    Actores = "Tom Hardy, Charlize Theron, Nicholas Hoult",
                    ImagenUrl = "https://via.placeholder.com/300x450/FF8C00/FFFFFF?text=Mad+Max",
                    FechaCreacion = DateTime.Now
                }
            };

            await context.Peliculas.AddRangeAsync(peliculas);
            await context.SaveChangesAsync();
        }
    }
}