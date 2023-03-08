using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using MovieStreamingApp.Entities;
using MovieStreamingApp.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace MovieStreamingApp.Controllers
{
	public class MovieController : Controller
	{
		public MovieController(MovieDbContext movieDbContext, IConfiguration configuration)
		{
			_movieDbContext = movieDbContext;
			_configuration = configuration;
		}

		[HttpGet()]
		public IActionResult List()
		{
			var allMovies = _movieDbContext.Movies
					.Include(m => m.Genre).
					Include(m => m.ProductionStudio)
					.OrderBy(m => m.Name).ToList();

			return View(allMovies);
		}

		[HttpGet()]
		public IActionResult Add()
		{
			MovieViewModel movieViewModel = new MovieViewModel()
			{
				Genres = _movieDbContext.Genres.OrderBy(g => g.Name).ToList(),
				Studios = _movieDbContext.Studios.OrderBy(s => s.Name).ToList(),
				ActiveMovie = new Movie()
			};

			return View(movieViewModel);
		}

		[HttpPost()]
		public IActionResult Add(MovieViewModel movieViewModel)
		{
			if (ModelState.IsValid)
			{
				// it's valid so we want to add the new movie to the DB:
				_movieDbContext.Movies.Add(movieViewModel.ActiveMovie);
				_movieDbContext.SaveChanges();
				return RedirectToAction("List", "Movie");
			}
			else
			{
				// it's invalid so we simply return the movie object
				// to the Edit view again:
				movieViewModel.Genres = _movieDbContext.Genres.OrderBy(g => g.Name).ToList();
				movieViewModel.Studios = _movieDbContext.Studios.OrderBy(s => s.Name).ToList();
				return View(movieViewModel);
			}
		}

		[HttpGet()]
		public IActionResult Edit(int id)
		{
			MovieViewModel movieViewModel = new MovieViewModel()
			{
				Genres = _movieDbContext.Genres.OrderBy(g => g.Name).ToList(),
				Studios = _movieDbContext.Studios.OrderBy(s => s.Name).ToList(),
				ActiveMovie = _movieDbContext.Movies.Find(id)
			};

			return View(movieViewModel);
		}

		[HttpPost()]
		public IActionResult Edit(MovieViewModel movieViewModel)
		{
			if (ModelState.IsValid)
			{
				// it's valid so we want to update the existing movie in the DB:
				_movieDbContext.Movies.Update(movieViewModel.ActiveMovie);
				_movieDbContext.SaveChanges();
				return RedirectToAction("List", "Movie");
			}
			else
			{
				movieViewModel.Genres = _movieDbContext.Genres.OrderBy(g => g.Name).ToList();
				movieViewModel.Studios = _movieDbContext.Studios.OrderBy(s => s.Name).ToList();
				return View(movieViewModel);
			}
		}

		[HttpGet()]
		public IActionResult Delete(int id)
		{
			// Find/retrieve/select the movie to edit and then pass it to the view:
			var movie = _movieDbContext.Movies.Find(id);
			return View(movie);
		}

		[HttpPost()]
		public IActionResult Delete(Movie movie)
		{
			// Simply remove the movie and then redirect back to the all movies view:
			_movieDbContext.Movies.Remove(movie);
			_movieDbContext.SaveChanges();
			return RedirectToAction("List", "Movie");
		}

		[HttpGet("/api/movies/request/{id}")]
		public IActionResult MakeMovieRequest(int id)
		{
			var streamCompany = _configuration.GetSection("StreamingCompanySettings").GetSection("Name").Value;
			string url = string.Format("/api/movie-notifications/{0}", id);

			HttpClient client = new HttpClient();
			using (var content = new StringContent(JsonConvert.SerializeObject(streamCompany), System.Text.Encoding.UTF8, "application/json"))
			{
				HttpResponseMessage result = client.PostAsync(url,content).Result;
				if (result.StatusCode == HttpStatusCode.Accepted)
				{
					var movie = _movieDbContext.Movies.Where(m => m.MovieId == id).FirstOrDefault();
					if (movie != null)
					{
						movie.StreamingStatus = true;
						_movieDbContext.Update(movie);
						_movieDbContext.SaveChanges();
					}
				}
			}
			return View("List");
		}

		private MovieDbContext _movieDbContext;
		private IConfiguration _configuration;

	}
}
