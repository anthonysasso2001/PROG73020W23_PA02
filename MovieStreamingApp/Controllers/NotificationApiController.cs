using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieProductionApp.Models;
using MovieStreamingApp.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieStreamingApp.Controllers
{
	[ApiController()]
	public class NotificationApiController : ControllerBase
	{
		public NotificationApiController(MovieDbContext movieDbContext, IConfiguration configuration)
		{
			_movieDbContext = movieDbContext;
			_configuration = configuration;
		}

		[HttpPost("/api/movies")]
		public IActionResult GetNewMovie([FromBody] MovieApiInfo newInfo)
		{
			Movie newMovie = new Movie()
			{
				Name = newInfo.Name,
				Year = newInfo.Year,
				Rating = newInfo.AverageRating,
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

		[HttpPost("/api/challenge")]
		public string RecieveChallenge(APIChallengeRequest challengeInfo)
		{
			var Guid = new Guid(_configuration.GetSection("ProductionStudioSettings").GetSection("API_Key").Value);

			string challengeAnswer = Guid.ToString() + challengeInfo.challengeString;

			return challengeAnswer;
		}

		private MovieDbContext _movieDbContext;
		private IConfiguration _configuration;
	}
}
