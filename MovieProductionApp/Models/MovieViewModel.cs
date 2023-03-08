using MovieProductionApp.Entities;

namespace MovieProductionApp.Models
{
    public class MovieViewModel
    {
        public List<Genre>? Genres { get; set; }

        public Movie ActiveMovie { get; set; }
    }
}
