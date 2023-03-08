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
			var movieReviews = _movieDbContext.MovieApiData
				.Include(m => m.Movie)
				.OrderByDescending(m => m.TimeOfOffer)
				.Select(m => m.Movie)
				.Select(m => m.Reviews).FirstOrDefault().ToList();

			//get average rating to send
			int? movieRating = 0;
			int count = 0;

			if (movieReviews.IsNullOrEmpty())
				return Json(null);

			foreach (var review in movieReviews)
			{
				movieRating += review.Rating;
				count++;
			}

			movieRating /= count;

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
					Rating = movieRating,
					GenreId = m.Movie.GenreId,
					ProductionStudio = m.ProductionStudio
				}).FirstOrDefault();

			return Json(movieData);
		}

		[HttpPost("/api/movie-notifications/{name}")]
		public JsonResult GetInterest([FromBody] StreamCompanyInfo streamCompany, string name)
		{
			var movieReviews = _movieDbContext.MovieApiData
				.Include(m => m.Movie)
				.OrderByDescending(m => m.TimeOfOffer)
				.Select (m => m.Movie)
				.Where(m => m.Name == name)
				.Select(m => m.Reviews).FirstOrDefault().ToList();

			//get average rating to send
			int? movieRating = 0;
			int count = 0;

			if (movieReviews.IsNullOrEmpty())
				return Json(null);

			foreach (var review in movieReviews)
			{
				movieRating += review.Rating;
				count++;
			}

			movieRating /= count;

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
					Rating = movieRating,
					GenreId = m.Movie.GenreId,
					ProductionStudio = m.ProductionStudio
				}).Where(m => m.Name == name).FirstOrDefault();

			movieData.StreamPartner = streamCompany.Name;

			return Json(null);
		}

		private MovieDbContext _movieDbContext;
		private NotifyAPIHandler _notifyHandler;
	}
}
