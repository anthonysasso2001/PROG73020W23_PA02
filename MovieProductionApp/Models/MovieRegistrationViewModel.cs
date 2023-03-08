using MovieProductionApp.Entities;

namespace MovieProductionApp.Models
{
	public class MovieRegistrationViewModel : StreamCompanyViewModel
	{
		public MovieApiInfo ActiveMovieInfo { get; set; }

		public bool MovieAvailability { get; set; }
	}
}
