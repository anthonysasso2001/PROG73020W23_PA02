using MovieProductionApp.Entities;

namespace MovieProductionApp.Models
{
    public class MoviesByGenreViewModel
    {
        public List<Movie>? Movies { get; set; }

        public string? ActiveGenreName { get; set; }
    }
}
