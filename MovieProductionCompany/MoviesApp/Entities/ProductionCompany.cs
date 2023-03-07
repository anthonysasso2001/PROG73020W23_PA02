namespace MoviesApp.Entities
{
	//producer class to handle broadcasting
	public class ProductionCompany : IObservable<MovieApiData>
	{
		List<IObserver<MovieApiData>> observers;
		private MovieApiData? _latestMovie;

		public ProductionStudio? ProductionStudio { get; set; }

		public ProductionCompany()
		{
			observers = new List<IObserver<MovieApiData>>();
			_latestMovie = null;
		}

		private class Unsubscriber : IDisposable
		{
			private List<IObserver<MovieApiData>> _observers;
			private IObserver<MovieApiData> _observer;

			public Unsubscriber(List<IObserver<MovieApiData>> observers,IObserver<MovieApiData> observer)
			{
				_observers = observers;
				_observer = observer;
			}
			public void Dispose()
			{
				if (!(null == _observer)) _observers.Remove(_observer);
			}
		}

		public IDisposable Subscribe(IObserver<MovieApiData> observer)
		{
			if (!observers.Contains(observer))
				observers.Add(observer);

			return new Unsubscriber(observers, observer);
		}

		public void NewMovie(Movie newMovie, MovieDbContext dbCon)
		{
			MovieApiData newMovieInfo = new MovieApiData();

			// update movie info with current time offer is created, and production studio information, and the movie.
			newMovieInfo.TimeOfOffer = DateTime.UtcNow;
			
			newMovieInfo.ProductionStudioId = ProductionStudio.ProductionStudioId;
			newMovieInfo.ProductionStudio = ProductionStudio;

			newMovieInfo.MovieId = newMovie.MovieId;
			newMovieInfo.Movie = newMovie;

			foreach (var observer in observers)
				observer.OnNext(newMovieInfo);

			dbCon.MovieApiData.Add(newMovieInfo);

			_latestMovie = newMovieInfo;
		}

		public bool IsNewMovie(MovieApiData checkMovie)
		{
			//if either the api id is the same, then this is not a new movie, only check latest though in case of re-releases etc.
			if (null != _latestMovie && _latestMovie.MovieId == checkMovie.MovieId)
				return false;
			else
				return true;
		}

		public void KillAPI()
		{
			foreach (var observer in observers)
				if (null != observer) observer.OnCompleted();
		}
	}
}
