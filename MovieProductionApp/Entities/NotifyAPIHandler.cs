using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Net;

namespace MovieProductionApp.Entities
{
	public class NotifyAPIHandler
	{
		public ProductionStudio ProductionStudio { get; set; }

		public List<StreamCompanyInfo> StreamCompanies { get; set; }

		public bool sendToOne(MovieApiInfo newMovie, string webAPI)
		{
			HttpClient client = new HttpClient();

			using (var content = new StringContent(JsonConvert.SerializeObject(newMovie), System.Text.Encoding.UTF8, "application/json"))
			{
				HttpResponseMessage result = client.PostAsync(webAPI, content).Result;
				if (result.StatusCode == HttpStatusCode.Created)
					return true;
				else
					return false;
			}
		}

		public virtual void SendNotification(MovieApiInfo newMovie)
		{
			foreach (var streamer in StreamCompanies)
				sendToOne(newMovie, streamer.webApiUrl.ToString());
		}

		public void registerMovie(Movie newMovie, MovieDbContext dbCon)
		{
			MovieApiData newMovieData = new MovieApiData();

			// update movie info with current time offer is created, and production studio information, and the movie.
			newMovieData.TimeOfOffer = DateTime.UtcNow;

			newMovieData.ProductionStudioId = ProductionStudio.ProductionStudioId;

			newMovieData.MovieId = newMovie.MovieId;

			dbCon.MovieApiData.Add(newMovieData);

			dbCon.SaveChanges();
			var rating = 0;

			if(!newMovieData.Movie.Reviews.IsNullOrEmpty())
				rating = (int)newMovieData.Movie.Reviews.Average(r => r.Rating).GetValueOrDefault();
			
			var newMovieInfo = new MovieApiInfo()
			{
				TimeOfOffer = newMovieData.TimeOfOffer,
				Name = new string(newMovieData.Movie.Name),
				Year = newMovieData.Movie.Year,
				AverageRating = rating,
				GenreName = newMovieData.Movie.Genre.Name,
				ProductionStudioName = newMovieData.ProductionStudio.Name
			};

			SendNotification(newMovieInfo);
		}
	}
}
