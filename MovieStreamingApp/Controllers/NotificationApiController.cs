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
			ProductionStudio studio = new ProductionStudio();
			if (!_movieDbContext.Studios.Where(p => p.Name == newInfo.ProductionStudioName).Any())
			{
				studio.Name = newInfo.Name;
				_movieDbContext.Studios.Add(studio);
				_movieDbContext.SaveChanges();
			}

			studio = _movieDbContext.Studios.Where(p => p.Name == newInfo.ProductionStudioName).FirstOrDefault();

			Movie newMovie = new Movie()
			{
				Name = newInfo.Name,
				Year = newInfo.Year,
				Rating = newInfo.AverageRating,
				GenreId = _movieDbContext.Genres.Where(g => g.Name == newInfo.GenreName).FirstOrDefault().GenreId,
				ProductionStudio = studio
			};

			Genre? addGenre = null;

			if(!_movieDbContext.Genres.Where(g => g.Name == newInfo.GenreName).Any())
			{
				addGenre = new Genre()
				{
					Name = newInfo.GenreName
				};
				_movieDbContext.Genres.Add(addGenre);
				_movieDbContext.SaveChanges();
			}

			_movieDbContext.Movies.Add(newMovie);

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
