using MovieStreamingApp.Entities;

namespace MovieStreamingApp.Models
{
    public class MovieViewModel
    {
        public List<Genre>? Genres { get; set; }

        public List<ProductionStudio>? Studios { get; set; }

        public Movie ActiveMovie { get; set; }

        public bool StreamState { get; set; }
    }
}
