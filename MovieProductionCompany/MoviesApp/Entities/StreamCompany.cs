using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MoviesApp.Entities
{
	public class StreamCompany : IObserver<MovieApiData>
	{
		public IDisposable unsubscriber;
		private bool first = true;
		private MovieApiData? _latestMovie = null;

		public StreamCompanyInfo? streamCompanyInfo { get; set; }

		public virtual void Subscribe(IObservable<MovieApiData> provider)
		{
			unsubscriber = provider.Subscribe(this);
		}

		public virtual void Unsubscribe() { unsubscriber.Dispose(); }

		public virtual void OnCompleted()
		{
			//what to do when no more movies
		}

		public virtual void OnError(Exception error)
		{
			Console.Error.WriteLine(error.ToString());
		}

		public virtual void OnNext(MovieApiData newMovie)
		{
			if(first)
			{
				_latestMovie = newMovie;
				first = false;
			}
			else
			{
				//trigger background service to send to api?
			}
		}
	}
}
