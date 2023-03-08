using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieProductionApp.Entities;
using MovieProductionApp.Models;
using System.Security.Cryptography;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;
using NuGet.Protocol;

namespace MovieProductionApp.Controllers
{
	public class StreamCompanyController : Controller
	{
		public StreamCompanyController(MovieDbContext movieDbContext)
		{
			_movieDbContext = movieDbContext;

			//kind of weird to have the API handler here but needed to add stream companies
			//properly, and this controller is kind of the "front-end" to that API anyways
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

		// GET: all streaming companies with their registered movies
		[HttpGet("/movie-registry")]
		public IActionResult GetMovieRegistry()
		{
			var movieRegistryData = _movieDbContext.MovieApiData
				 .Include(m => m.Movie)
				 .Include(m => m.ProductionStudio)
				 .Include(m => m.StreamPartner)
				 .OrderByDescending(m => m.TimeOfOffer)
				 .Where(m => m.Availability == true)
				 .Select(m => new MovieRegistrationViewModel()
				 {
					 ActiveMovieInfo = new MovieApiInfo()
					 {
						 TimeOfOffer = m.TimeOfOffer,
						 StreamPartner = m.StreamPartner.Name,
						 Name = m.Movie.Name,
						 Year = m.Movie.Year,
						 AverageRating = (int)m.Movie.Reviews.Average(r => r.Rating).GetValueOrDefault(),
						 GenreId = m.Movie.GenreId,
						 ProductionStudio = m.ProductionStudio
					 },
					 MovieAvailability = m.Availability
				 }).ToList();

			return View("FullMovieRegistry", movieRegistryData);
		}

		//GET: one streaming partner with their api information etc.
		[HttpGet("/streaming-companies/{id}")]
		public IActionResult GetStreamingCompanyById(int id)
		{
			var streamingCompany = _movieDbContext.StreamCompanies
				.Select(s => new StreamCompanyViewModel()
				{
					ActiveStreamCompany = s
				}).Where(s => s.ActiveStreamCompany.StreamCompanyInfoId == id).FirstOrDefault();

			return View("StreamCompany", streamingCompany);
		}

		[HttpPost("/streaming-companies/verify/{id}")]
		public IActionResult VerifyStreamingCompany(int id)
		{
			//send verification request
			var streamCompanyInfo = _movieDbContext.StreamCompanies.Where(s => s.StreamCompanyInfoId == id).FirstOrDefault();

			if (null == streamCompanyInfo)
				return RedirectToAction("GetStreamingCompanyById", id);

			HttpClient httpClient = new HttpClient();
			string url = streamCompanyInfo.challengeUrl;
			string randString = RandomNumberGenerator.GetBytes(64).ToString();

			APIChallengeRequest apichallenge = new APIChallengeRequest()
			{
				challengeString = randString,
			};

			using (var content = new StringContent(JsonConvert.SerializeObject(apichallenge), System.Text.Encoding.UTF8, "application/json"))
			{
				HttpResponseMessage result = httpClient.PostAsync(url, content).Result;
				string challengeAnswer = streamCompanyInfo.StreamGUID.ToString() + randString;
				if (result.Content.ReadAsStream().ToString() == challengeAnswer)
				{
					streamCompanyInfo.verificationStatus = true;

					_movieDbContext.Update(streamCompanyInfo);

					_movieDbContext.SaveChanges();
				}
			}

			return View("StreamCompany", new StreamCompanyViewModel() { ActiveStreamCompany = streamCompanyInfo });
		}

		[HttpGet("/streaming-companies/register")]
		public IActionResult RegisterStreamingCompanyRequest()
		{
			var newCompany = new StreamCompanyViewModel();
			return View("NewStreamCompany", newCompany);
		}

		[HttpPost("/streaming-companies")]
		public IActionResult AddStreamingCompany(StreamCompanyViewModel streamCompany)
		{
			if (null == streamCompany.ActiveStreamCompany ||
				null == streamCompany.ActiveStreamCompany.Name ||
				null == streamCompany.ActiveStreamCompany.webApiUrl ||
				null == streamCompany.ActiveStreamCompany.challengeUrl)
				return BadRequest();

			//generate a custom guid or regenerate if it already exists
			bool uniqueGuid = false;
			while (!uniqueGuid)
			{
				streamCompany.ActiveStreamCompany.StreamGUID = Guid.NewGuid();
				// if it exists re-run loop
				if (!_movieDbContext.StreamCompanies.Any(s => s.StreamGUID == streamCompany.ActiveStreamCompany.StreamGUID))
					uniqueGuid = true;
			}
			_movieDbContext.StreamCompanies.Add(streamCompany.ActiveStreamCompany);
			//this is redundant but just to ensure notify handler is up to data, even if it is regenerated each time
			_movieDbContext.SaveChanges();


			_notifyHandler.StreamCompanies.Add(
				_movieDbContext.StreamCompanies.Where(s => s.Name == streamCompany.ActiveStreamCompany.Name).FirstOrDefault());

			return View("StreamCompany", new StreamCompanyViewModel() { ActiveStreamCompany = streamCompany.ActiveStreamCompany });
		}
		private MovieDbContext _movieDbContext;
		private NotifyAPIHandler _notifyHandler;
	}
}

/*
		// GET api/streamers/exampleStream
		[HttpGet("/api/streamCompanies/{name}")]
		public JsonResult GetStreamingCompanyByName(string name)
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
		[HttpPost("/api/streamCompanies")]
		public IActionResult AddStreamingCompany([FromBody] StreamCompanyInfo companyInfo)
		{
			if (null == companyInfo || null == companyInfo.Name || null == companyInfo.webApiUrl || null == companyInfo.challengeUrl)
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
		[HttpPut("/api/streamCompanies/{name}")]
		public IActionResult UpdateStreamingCompany(string name, [FromBody] StreamCompanyInfo updateInfo)
		{
			_movieDbContext.StreamCompanies.Update(updateInfo);
			_movieDbContext.SaveChanges();

			return CreatedAtAction(
				nameof(StreamCompanyInfo),
				new { Name = updateInfo.Name },
				updateInfo);
		}

		// DELETE api/<NotifyAPIController>/5
		[HttpDelete("/api/streamCompanies/{name}")]
		public IActionResult DeleteStreamingCompany(string name)
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

 */