using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MoviesApp.Entities;
using System.IO;
using System.Text.Json.Nodes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoviesApp.Controllers
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

		// GET: api/streamers
		[HttpGet("/api/streamers")]
		public JsonResult GetAllStreamers()
		{
			var streamers = _movieDbContext.StreamCompanies
				.OrderByDescending(s => s.Name)
				.Select(s => new StreamCompanyInfo()
				{
					Name = s.Name,
					webApiUrl = s.webApiUrl
				}).ToList();

			return Json(streamers);
		}

		// GET api/streamers/exampleStream
		[HttpGet("/api/streamers/{name}")]
		public JsonResult GetStreamerByValue(string name)
		{
			var streamer = _movieDbContext.StreamCompanies
				.Select(s => new StreamCompanyInfo()
				{
					Name = s.Name,
					webApiUrl = s.webApiUrl
				}).Where(s => s.Name.Equals(name))
				.FirstOrDefault();

			return Json(streamer);
		}

		// POST api/<NotifyAPIController>
		[HttpPost("/api/streamers")]
		public IActionResult Post([FromBody] StreamCompanyInfo companyInfo)
		{
			if (null == companyInfo || null == companyInfo.Name || null == companyInfo.webApiUrl)
				return BadRequest();

			_movieDbContext.StreamCompanies.Add(companyInfo);
			//this is redundant but just to ensure notify handler is up to data, even if it is regenerated each time
			_notifyHandler.StreamCompanies.Add(companyInfo);

			return CreatedAtAction(
				nameof(StreamCompanyInfo),
				new { Name = companyInfo.Name },
				companyInfo);
		}

		// PUT api/<NotifyAPIController>/5
		[HttpPut("/api/streamers/{name}")]
		public IActionResult Put(string name, [FromBody] StreamCompanyInfo updateInfo)
		{
			_movieDbContext.StreamCompanies.Update(updateInfo);
			_movieDbContext.SaveChanges();

			return CreatedAtAction(
				nameof(StreamCompanyInfo),
				new { Name = updateInfo.Name },
				updateInfo);
		}

		// DELETE api/<NotifyAPIController>/5
		[HttpDelete("/api/streamers/{name}")]
		public IActionResult Delete(string name)
		{
			var delInfo = _movieDbContext.StreamCompanies
				.Select(s => new StreamCompanyInfo()
				{
					Name = s.Name,
					webApiUrl = s.webApiUrl
				}).Where(s => s.Name.Equals(name))
				.FirstOrDefault();

			if (delInfo == null)
				return BadRequest();

			_movieDbContext.Remove(delInfo);

			var deleteCompany = _notifyHandler.StreamCompanies.Find(s => s == delInfo);

			if (deleteCompany == null)
				return BadRequest();

			_notifyHandler.StreamCompanies.Remove(deleteCompany);

			_movieDbContext.SaveChanges();
			return Ok();
		}

		// GET api/streamers/exampleStream
		[HttpGet("/api/studio")]
		public JsonResult GetStudio()
		{
			var studio = _notifyHandler.ProductionStudio;

			return Json(studio);
		}

		[HttpGet("/api/notification")]
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

		[HttpPost("/api/notification/{name}")]
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
		private readonly IAuthorizationService _authorizationService;
		private NotifyAPIHandler _notifyHandler;
	}
}
