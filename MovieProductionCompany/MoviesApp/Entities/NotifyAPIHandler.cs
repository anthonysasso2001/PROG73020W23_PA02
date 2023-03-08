using Newtonsoft.Json;
using System.Net;

namespace MoviesApp.Entities
{
	public class NotifyAPIHandler
	{
		public ProductionStudio ProductionStudio { get; set; }

		public List<StreamCompanyInfo> StreamCompanies { get; set; }

		public bool sendToOne(MovieApiData newMovie, string webAPI)
		{
			HttpClient client = new HttpClient();

			using (var content = new StringContent(JsonConvert.SerializeObject(newMovie), System.Text.Encoding.UTF8, webAPI))
			{
				HttpResponseMessage result = client.PostAsync(webAPI, content).Result;
				if (result.StatusCode == HttpStatusCode.Created)
					return true;
				else
					return false;
			}
		}

		public virtual void SendNotification(MovieApiData newMovie)
		{
			foreach (var streamer in StreamCompanies)
				sendToOne(newMovie, streamer.webApiUrl.ToString());
		}

		public void registerMovie(Movie newMovie, MovieDbContext dbCon)
		{
			MovieApiData newMovieInfo = new MovieApiData();

			// update movie info with current time offer is created, and production studio information, and the movie.
			newMovieInfo.TimeOfOffer = DateTime.UtcNow;

			newMovieInfo.ProductionStudioId = ProductionStudio.ProductionStudioId;
			newMovieInfo.ProductionStudio = ProductionStudio;

			newMovieInfo.MovieId = newMovie.MovieId;
			newMovieInfo.Movie = newMovie;

			dbCon.MovieApiData.Add(newMovieInfo);

			dbCon.SaveChanges();
		}
	}
}
