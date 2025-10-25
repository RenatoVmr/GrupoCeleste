using Microsoft.ML.Data;

namespace GrupoCeleste.Models
{
    public class MovieRating
    {
        [LoadColumn(0)]
        public uint UserId { get; set; }

        [LoadColumn(1)]
        public uint MovieId { get; set; }

        [LoadColumn(2)]
        public float Rating { get; set; }
    }

    public class MovieRatingPrediction
    {
        public float Score { get; set; }
    }
}