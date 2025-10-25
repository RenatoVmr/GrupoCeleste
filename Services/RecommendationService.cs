using Microsoft.ML;
using Microsoft.ML.Trainers;
using GrupoCeleste.Models;
using GrupoCeleste.Data;
using System.Linq;
using System.Collections.Generic;

namespace GrupoCeleste.Services
{
    public class RecommendationService
    {
        private readonly MLContext _mlContext = new MLContext();
        private ITransformer? _model;

        public RecommendationService()
        {
            TrainModel();
        }

        private void TrainModel()
        {
            // Datos de ejemplo para entrenamiento (en producción usa datos reales)
            var ratings = new List<MovieRating>
            {
                new MovieRating { UserId = 1, MovieId = 1, Rating = 5.0f },
                new MovieRating { UserId = 1, MovieId = 2, Rating = 3.0f },
                new MovieRating { UserId = 1, MovieId = 3, Rating = 4.0f },
                new MovieRating { UserId = 2, MovieId = 1, Rating = 4.0f },
                new MovieRating { UserId = 2, MovieId = 2, Rating = 5.0f },
                new MovieRating { UserId = 2, MovieId = 4, Rating = 3.0f },
                new MovieRating { UserId = 3, MovieId = 1, Rating = 2.0f },
                new MovieRating { UserId = 3, MovieId = 3, Rating = 5.0f },
                new MovieRating { UserId = 3, MovieId = 4, Rating = 4.0f },
                new MovieRating { UserId = 4, MovieId = 2, Rating = 4.0f },
                new MovieRating { UserId = 4, MovieId = 3, Rating = 3.0f },
                new MovieRating { UserId = 4, MovieId = 5, Rating = 5.0f }
            };

            var data = _mlContext.Data.LoadFromEnumerable(ratings);

            var pipeline = _mlContext.Transforms.Conversion.MapValueToKey("UserId")
                .Append(_mlContext.Transforms.Conversion.MapValueToKey("MovieId"))
                .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(
                    new MatrixFactorizationTrainer.Options
                    {
                        MatrixColumnIndexColumnName = "UserId",
                        MatrixRowIndexColumnName = "MovieId",
                        LabelColumnName = "Rating",
                        NumberOfIterations = 20,
                        ApproximationRank = 100
                    }));

            _model = pipeline.Fit(data);
        }

        public List<Pelicula> GetRecommendationsForUser(string userEmail, ApplicationDbContext db)
        {
            if (_model == null) return new List<Pelicula>();
            
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<MovieRating, MovieRatingPrediction>(_model);

            // Obtener películas no vistas por el usuario (simplificado)
            var allMovies = db.Peliculas.ToList();

            var recommendations = allMovies
                .Select(m => new
                {
                    Movie = m,
                    Prediction = predictionEngine.Predict(new MovieRating { UserId = 1, MovieId = (uint)m.Id })
                })
                .OrderByDescending(p => p.Prediction.Score)
                .Take(6)
                .Select(p => p.Movie)
                .ToList();

            return recommendations;
        }
    }
}