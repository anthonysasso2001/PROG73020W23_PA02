using Microsoft.AspNetCore.Mvc;

namespace MoviesApp.Entities
{
    public class MovieStudio : IObservable<MovieInfo>
    {
        List<IObserver<MovieInfo>> observers;
        private MovieInfo? previous;
        public ProductionStudio MovieStudioId { get; set; }

        public MovieStudio()
        {
            observers = new List<IObserver<MovieInfo>>();
            previous = null;
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<MovieInfo>> _observers;
            private IObserver<MovieInfo> _observer;

            public Unsubscriber(List<IObserver<MovieInfo>> observers, IObserver<MovieInfo> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (!(null == _observer)) _observers.Remove(_observer);
            }
        }

        public IDisposable Subscribe(IObserver<MovieInfo> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);

            return new Unsubscriber(observers, observer);
        }

        public void addNewMovie(MovieInfo newMovieInfo)
        {
            foreach (var observer in observers)
                observer.OnNext(newMovieInfo);

            previous = newMovieInfo;
        }

        public bool isNewMovie(Movie checkMovie)
        {
            if (previous.MovieId != checkMovie.MovieId)
                return true;
            else
                return false;
        }

        public void KillAPI()
        {
            foreach (var observer in observers)
                if (null != observer) observer.OnCompleted();
        }
    }
}
