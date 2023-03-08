using MovieProductionApp.Entities;

namespace MovieProductionApp.Models
{
    public class BaseMovieViewModel
    {
        public Movie? ActiveMovie { get; set; }

        public double AverageRating { get; set; }
    }
}
