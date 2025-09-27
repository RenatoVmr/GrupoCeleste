namespace GrupoCeleste.Models.ViewModels
{
    public class PeliculasViewModel
    {
        public IEnumerable<Pelicula> Peliculas { get; set; } = new List<Pelicula>();
        
        // Filtros
        public string? BusquedaTitulo { get; set; }
        public string? GeneroSeleccionado { get; set; }
        public string? DirectorSeleccionado { get; set; }
        public int? AnioDesde { get; set; }
        public int? AnioHasta { get; set; }
        public decimal? CalificacionMinima { get; set; }
        
        // Ordenamiento
        public string OrdenarPor { get; set; } = "Titulo";
        public bool OrdenDescendente { get; set; } = false;
        
        // Paginación
        public int PaginaActual { get; set; } = 1;
        public int PeliculasPorPagina { get; set; } = 12;
        public int TotalPeliculas { get; set; }
        public int TotalPaginas => (int)Math.Ceiling((double)TotalPeliculas / PeliculasPorPagina);
        
        // Listas para filtros
        public IEnumerable<string> GenerosDisponibles { get; set; } = new List<string>();
        public IEnumerable<string> DirectoresDisponibles { get; set; } = new List<string>();
    }

    public class DetallePeliculaViewModel
    {
        public Pelicula Pelicula { get; set; } = new Pelicula();
        public IEnumerable<Pelicula> PeliculasSimilares { get; set; } = new List<Pelicula>();
    }
}