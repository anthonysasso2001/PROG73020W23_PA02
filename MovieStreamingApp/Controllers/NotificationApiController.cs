using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieStreamingApp.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieStreamingApp.Controllers
{
	[ApiController()]
	public class NotificationApiController : ControllerBase
	{
		public NotificationApiController(MovieDbContext movieDbContext)
		{
			_movieDbContext = movieDbContext;
		}

		[HttpPost("/api/movies")]
		public IActionResult GetNewMovie([FromBody] MovieApiInfo newInfo)
		{
			Movie newMovie = new Movie()
			{
				Name = newInfo.Name,
				Year = newInfo.Year,
				Rating = newInfo.Rating,
				GenreId = newInfo.GenreId,
				ProductionStudio = newInfo.ProductionStudio
			};

			var addGenre = _movieDbContext.Genres
				.Select(g => new Genre()
				{
					GenreId = g.GenreId,
					Name = g.Name
				}).Where(g => g.GenreId == newInfo.GenreId)
				.FirstOrDefault();

			newMovie.Genre = addGenre;

			_movieDbContext.Add(newMovie);

			_movieDbContext.SaveChanges();

			return Ok();
		}

		private MovieDbContext _movieDbContext;
	}
}
