using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using MoviesApp.Entities;
using MoviesApp.Models;
using NuGet.Protocol;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MoviesApp.Controllers
{
	[ApiController()]
	public class MovieAPIController : Controller
	{

		private MovieDbContext _movieDbContext;
		private NotifierContext _notifierContext;

		public MovieAPIController(MovieDbContext movieDbContext, NotifierContext notifierContext)
		{
			_movieDbContext = movieDbContext;
			_notifierContext = notifierContext;
		}

		// GET streaming companies, and api paths (probably not proper but just a lab so not too important
		[HttpGet("/api/MSS")]
		public IActionResult Get()
		{
			var streamers = _notifierContext.MovieStreamers
				.OrderByDescending(s => s.Name)
				.Select(s => new MovieStreamer()
				{
					MovieStreamerId = s.MovieStreamerId,
					Name = s.Name
				}
				).ToList();

			return Json(streamers);
		}

		// GET specific streaming company
		[HttpGet("/api/MSS/{id}")]
		public IActionResult GetById(string id)
		{
			var streamer = _notifierContext.MovieStreamers
				.Include(s => s.Name)
				.Select(s => new MovieStreamer()
				{
					MovieStreamerId = s.MovieStreamerId,
					Name = s.Name
				}
				).Where(s => s.MovieStreamerId == id)
				.FirstOrDefault();

			return Json(streamer);
		}

		// POST api/
		[HttpPost("/api/MSS")]
		public IActionResult Post([FromBody] string web_URL, string name)
		{
			MovieStreamer newStreamer = new MovieStreamer();
			newStreamer.MovieStreamerId = web_URL;
			newStreamer.Name = name;

			var studio = _notifierContext.Studio.First();

			newStreamer.Subscribe(studio);

			_notifierContext.MovieStreamers.Add(newStreamer);
			_notifierContext.SaveChanges();

			return Json(newStreamer);
		}

		// PUT api/<MovieAPIController>/5
		[HttpPut("/api/MSS/{id}")]
		public IActionResult Put(string web_URL, [FromBody] string name)
		{
			var toUpdate = _notifierContext.MovieStreamers.FirstOrDefault(s => s.MovieStreamerId == web_URL);

			toUpdate.MovieStreamerId = web_URL;
			toUpdate.Name = name;

			_notifierContext.Update(toUpdate);

			return Json(toUpdate);
		}

		// DELETE api/<MovieAPIController>/5
		[HttpDelete("/api/MSS/{id}")]
		public IActionResult Delete(string web_URL)
		{
			var toDelete = _notifierContext.MovieStreamers.FirstOrDefault(s => s.MovieStreamerId == web_URL);

			toDelete.Unsubscribe();

			_notifierContext.Remove(toDelete);

			return RedirectToAction("Index");
		}
	}
}
