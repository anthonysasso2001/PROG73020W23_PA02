using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Entities;
using MoviesApp.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoviesApp.Controllers
{
	[ApiController()]
	public class NotifyAPIController : Controller
	{

		public NotifyAPIController(MovieDbContext dbCont, APIBroadcastService apiService)
		{
			_dbContext = dbCont;
			_apiService = apiService;
		}

		// GET: api/streamers
		[HttpGet("/api/streamers")]
		public JsonResult GetAllStreamers()
		{
			var streamers = _dbContext.StreamCompanies
				.OrderByDescending(s => s.Name)
				.Select(s => new StreamCompanyInfo()
				{
					Name = s.Name,
					webApi = s.webApi
				}).ToList();

			return Json(streamers);
		}

		// GET api/streamers/exampleStream
		[HttpGet("/api/streamers/{name}")]
		public JsonResult GetStreamerByValue(string name)
		{
			var streamer = _dbContext.StreamCompanies
				.Select(s => new StreamCompanyInfo()
				{
					Name = s.Name,
					webApi = s.webApi
				}).Where(s => s.Name.Equals(name))
				.FirstOrDefault();

			return Json(streamer);
		}

		// POST api/<NotifyAPIController>
		[HttpPost]
		public IActionResult Post([FromBody] StreamCompanyInfo companyInfo)
		{
			var newStreamer = new StreamCompany();
			newStreamer.streamCompanyInfo = companyInfo;

			if (null == companyInfo || null == companyInfo.Name || null == companyInfo.webApi)
				return BadRequest();

			_dbContext.StreamCompanies.Add(companyInfo);
			_apiService.thisCompany.Subscribe(newStreamer);
			_apiService.streamers.Add(newStreamer);

			return CreatedAtAction(
				nameof(StreamCompanyInfo),
				new { Name = newStreamer.streamCompanyInfo.Name },
				newStreamer.streamCompanyInfo);
		}

		// PUT api/<NotifyAPIController>/5
		[HttpPut("/api/streamers/{name}")]
		public IActionResult Put(string name, [FromBody] StreamCompanyInfo updateInfo)
		{
			_dbContext.StreamCompanies.Update(updateInfo);
			_dbContext.SaveChanges();

			return CreatedAtAction(
				nameof(StreamCompanyInfo),
				new { Name = updateInfo.Name },
				updateInfo);
		}

		// DELETE api/<NotifyAPIController>/5
		[HttpDelete("/api/streamers/{name}")]
		public IActionResult Delete(string name)
		{
			var delInfo = _dbContext.StreamCompanies
				.Select(s => new StreamCompanyInfo()
				{
					Name = s.Name,
					webApi = s.webApi
				}).Where(s => s.Name.Equals(name))
				.FirstOrDefault();

			if (delInfo == null)
				return BadRequest();

			_dbContext.Remove(delInfo);

			var deleteCompany = _apiService.streamers.Find(s => s.streamCompanyInfo ==  delInfo);

			if (deleteCompany == null)
				return BadRequest();

			deleteCompany.Unsubscribe();

			_apiService.streamers.Remove(deleteCompany);

			_dbContext.SaveChanges();
			return Ok();


		}

		private MovieDbContext _dbContext;
		private APIBroadcastService _apiService;
	}
}
