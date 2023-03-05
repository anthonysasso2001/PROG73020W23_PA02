namespace MoviesApp.Entities
{
    public class MovieStreamer : IObserver<MovieInfo>
    {
        private IDisposable unsubscriber;
        private bool first = true;
        private MovieInfo last;

        public string MovieStreamerId { get; set; }
        public string Name { get; set; }

        public virtual void Subscribe(IObservable<MovieInfo> provider)
        {
            unsubscriber = provider.Subscribe(this);
        }

        public virtual void Unsubscribe() { unsubscriber.Dispose(); }

        public virtual void OnCompleted()
        {
            Console.WriteLine("No more movies");
        }

        public virtual void OnError(Exception error)
        {
            Console.WriteLine(error);
        }

        public virtual void OnNext(MovieInfo newMovie)
        {
            if (first)
            {
                last = newMovie;
                first = false;
            }
            else
            {
                //send movie info to web api stored
            }
        }
    }
}
