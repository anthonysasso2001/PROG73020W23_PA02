using MovieProductionApp.Entities;

namespace MovieProductionApp.Models
{
    public class MovieDetailViewModel : BaseMovieViewModel
    {
        public Review? NewReview { get; set; }
    }
}
