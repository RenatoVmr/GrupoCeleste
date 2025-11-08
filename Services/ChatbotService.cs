using GrupoCeleste.Data;
using GrupoCeleste.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace GrupoCeleste.Services
{
    public class ChatbotService
    {
        private readonly ApplicationDbContext _context;
        private readonly Dictionary<string, string[]> _keywords;

        public ChatbotService(ApplicationDbContext context)
        {
            _context = context;
            _keywords = InitializeKeywords();
        }

        public async Task<ChatResponse> ProcessMessageAsync(string message, string sessionId)
        {
            var cleanMessage = message.ToLower().Trim();

            // Respuestas de saludo
            if (IsGreeting(cleanMessage))
            {
                return new ChatResponse
                {
                    Message = "¡Hola! 👋 Soy tu asistente de CineVerse. Puedo ayudarte a encontrar películas perfectas para ti. ¿Qué género te gusta o qué tipo de película buscas?",
                    Success = true
                };
            }

            // Búsqueda por género
            if (IsGenreQuery(cleanMessage, out string genre))
            {
                var movies = await GetMoviesByGenreAsync(genre);
                return new ChatResponse
                {
                    Message = $"¡Excelente elección! Aquí tienes algunas películas de {genre} que te pueden gustar:",
                    RecommendedMovies = movies,
                    Success = true
                };
            }

            // Búsqueda por título
            if (IsMovieSearch(cleanMessage, out string movieTitle))
            {
                var movie = await SearchMovieByTitleAsync(movieTitle);
                if (movie != null)
                {
                    return new ChatResponse
                    {
                        Message = $"¡Encontré la película! '{movie.Titulo}' - {movie.Descripcion}",
                        RecommendedMovies = new List<Pelicula> { movie },
                        Success = true
                    };
                }
            }

            // Recomendaciones generales
            if (IsRecommendationRequest(cleanMessage))
            {
                var movies = await GetRandomRecommendationsAsync();
                return new ChatResponse
                {
                    Message = "Aquí tienes algunas películas populares que podrían interesarte:",
                    RecommendedMovies = movies,
                    Success = true
                };
            }

            // Respuesta por defecto
            return new ChatResponse
            {
                Message = "Entiendo que estás buscando información sobre películas. Puedes preguntarme por géneros (acción, comedia, drama), buscar una película específica, o pedirme recomendaciones. ¿En qué puedo ayudarte? 🎬",
                Success = true
            };
        }

        private Dictionary<string, string[]> InitializeKeywords()
        {
            return new Dictionary<string, string[]>
            {
                ["greeting"] = new[] { "hola", "hello", "hi", "buenos días", "buenas tardes", "hey" },
                ["genres"] = new[] { "acción", "comedia", "drama", "terror", "ciencia ficción", "romance", "thriller", "aventura" },
                ["recommendations"] = new[] { "recomienda", "sugiere", "qué ver", "que ver", "buenas películas", "mejores películas" },
                ["search"] = new[] { "buscar", "encontrar", "película", "film", "movie" }
            };
        }

        private bool IsGreeting(string message)
        {
            return _keywords["greeting"].Any(keyword => message.Contains(keyword));
        }

        private bool IsGenreQuery(string message, out string genre)
        {
            genre = string.Empty;
            foreach (var genreKeyword in _keywords["genres"])
            {
                if (message.Contains(genreKeyword))
                {
                    genre = char.ToUpper(genreKeyword[0]) + genreKeyword.Substring(1);
                    return true;
                }
            }
            return false;
        }

        private bool IsMovieSearch(string message, out string movieTitle)
        {
            movieTitle = string.Empty;
            if (_keywords["search"].Any(keyword => message.Contains(keyword)))
            {
                // Extraer el título después de palabras clave como "buscar", "película"
                var pattern = @"(?:buscar|película|film|movie)\s+(.+)";
                var match = Regex.Match(message, pattern, RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    movieTitle = match.Groups[1].Value.Trim();
                    return !string.IsNullOrEmpty(movieTitle);
                }
            }
            return false;
        }

        private bool IsRecommendationRequest(string message)
        {
            return _keywords["recommendations"].Any(keyword => message.Contains(keyword));
        }

        private async Task<List<Pelicula>> GetMoviesByGenreAsync(string genre)
        {
            return await _context.Peliculas
                .Where(p => p.Genero.ToLower().Contains(genre.ToLower()))
                .Take(3)
                .ToListAsync();
        }

        private async Task<Pelicula?> SearchMovieByTitleAsync(string title)
        {
            return await _context.Peliculas
                .FirstOrDefaultAsync(p => p.Titulo.ToLower().Contains(title.ToLower()));
        }

        private async Task<List<Pelicula>> GetRandomRecommendationsAsync()
        {
            return await _context.Peliculas
                .OrderBy(p => Guid.NewGuid())
                .Take(3)
                .ToListAsync();
        }
    }
}