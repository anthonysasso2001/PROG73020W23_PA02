using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieProductionApp.Entities;
using System.IO;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieProductionApp.Controllers
{
	[ApiController()]
	public class NotifyAPIController : Controller
	{
		public NotifyAPIController(MovieDbContext movieDbContext)
		{
			_movieDbContext = movieDbContext;

			_notifyHandler = new NotifyAPIHandler()
			{
				ProductionStudio = _movieDbContext.ProductionStudios
				.Select(p => new ProductionStudio()
				{
					Name = p.Name,
					ProductionStudioId = p.ProductionStudioId
				}).First(),
				StreamCompanies = _movieDbContext.StreamCompanies
				.Select(i => new StreamCompanyInfo()
				{
					Name = i.Name,
					webApiUrl = i.webApiUrl
				}).ToList()
			};
		}

		// GET api/streamers/exampleStream
		[HttpGet("/api/studio-information")]
		public JsonResult GetStudio()
		{
			var studio = _notifyHandler.ProductionStudio;

			return Json(studio);
		}

		[HttpGet("/api/movie-notifications")]
		public JsonResult GetNotification()
		{
			var movieData = _movieDbContext.MovieApiData
				.Include(m => m.Movie)
				.Include(m => m.ProductionStudio)
				.Include(m => m.StreamPartner)
				.OrderByDescending(m => m.TimeOfOffer)
				.Where(m => m.Availability == true)
				.Select(m => new MovieApiInfo()
				{
					TimeOfOffer = m.TimeOfOffer,
					StreamPartner = m.StreamPartner.Name,
					Name = m.Movie.Name,
					Year = m.Movie.Year,
					AverageRating = (int) m.Movie.Reviews.Average(r => r.Rating).GetValueOrDefault(),
					GenreName = m.Movie.Genre.Name,
					ProductionStudioName = m.ProductionStudio.Name
				}).FirstOrDefault();

			return Json(movieData);
		}

		[HttpPost("/api/movie-notifications/{movieName}")]
		public JsonResult SendInterest(string movieName, [FromBody]string companyName)
		{
			if (!_movieDbContext.StreamCompanies.Where(m => m.Name == companyName).Any())
				return null;

			var movieData = _movieDbContext.MovieApiData
				.Include(m => m.Movie)
				.Include(m => m.ProductionStudio)
				.Include(m => m.StreamPartner)
				.OrderByDescending(m => m.TimeOfOffer)
				.Select(m => new MovieApiInfo()
				{
					TimeOfOffer = m.TimeOfOffer,
					StreamPartner = m.StreamPartner.Name,
					Name = m.Movie.Name,
					Year = m.Movie.Year,
                    AverageRating = (int)m.Movie.Reviews.Average(r => r.Rating).GetValueOrDefault(),
                    GenreName = m.Movie.Genre.Name,
					ProductionStudioName = m.ProductionStudio.Name
				}).Where(m => m.Name == movieName).FirstOrDefault();

			if (movieData.StreamPartner != null)
			{
				return Json(null);
			}

			movieData.StreamPartner = companyName;

			return Json(movieData);
		}

		private MovieDbContext _movieDbContext;
		private NotifyAPIHandler _notifyHandler;
	}
}
