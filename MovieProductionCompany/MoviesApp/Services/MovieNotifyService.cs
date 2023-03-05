using Microsoft.EntityFrameworkCore;
using MoviesApp.Entities;
using MoviesApp.Models;
using System.Collections.Specialized;

namespace MoviesApp.Services
{
	public class MovieNotifyService : BackgroundService
	{

		public MovieNotifyService(IServiceProvider provider, MovieDbContext movieDbContext)
		{
			_provider = provider;
			_movieDbContext = movieDbContext;
			movieNotifyer = new MovieStudio();
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				using (var scope = _provider.CreateScope())
				{
					Console.WriteLine($"Checking for new Movies");

					//check here
					MovieInfo newMovie = new MovieInfo();
					var movie = _movieDbContext.Movies.Include(m => m.Genre)
						.Include(m => m.Reviews)
						.Include(m => m.Castings).ThenInclude(c => c.Actor)
						.Last();

					if (this.movieNotifyer.isNewMovie(movie))
					{
						//notify observers
						movieNotifyer.addNewMovie(newMovie);
					}
					else
					{
						//nothing?
					}

					await Task.Delay(_refreshInterval, stoppingToken);
				}
			}
		}

		private TimeSpan _refreshInterval = TimeSpan.FromSeconds(30);

		private IServiceProvider _provider;

		private MovieStudio movieNotifyer;

		private MovieDbContext _movieDbContext;
	}
}
