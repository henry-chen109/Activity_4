using System;
namespace MovieTracker
{
    interface IPrint {
        void PrintData();
    }

    public class Movie : IPrint
    {
        private string _className = "Movie Class";
        private int _serialNumber;
        public string Title { get; set; }
        private DateTime DateAdded { get; set; }

        public Movie(string title, DateTime dateAdded, int serialNum) 
        {
            Title = title;
            DateAdded = dateAdded;
            SerialNumber = serialNum;
        }

        public string ClassName
        {
            get 
            {
                return _className;
            }
        }

        private int SerialNumber {
            set {
                _serialNumber = value;
            }
        }

        public void PrintData() 
        {
            Console.Write($"Movie Title: {Title}\nDate Added: {DateAdded}\nClass Name: {ClassName}\nSerial Number: {_serialNumber}\n");
        }
    }

    public class MovieTrackerEventArgs : EventArgs {
        public Movie MovieInfo { get; set; }
    }

    public class MovieTracker : IPrint
    {
        private List<Movie> MovieList = new List<Movie>();
        
        public void OnAddMovie(object sender,  MovieTrackerEventArgs e) {
            MovieList.Add(e.MovieInfo);
            Console.WriteLine($"You have successfully added the movie '{e.MovieInfo.Title}'\n");
        }

        public void PrintData() {
            Console.WriteLine("\nMovies Stored\n-------------");
            if (MovieList.Count == 0) {
                Console.WriteLine("No Movies Currently Stored\n");
                Thread.Sleep(2000);
                return;
            }

            foreach(var movie in MovieList.Select( (value, index) => new { index , value })) {
                Console.WriteLine($"Movie {movie.index + 1}: {movie.value.Title}");
            }
        }

    }

    public class MovieTrackerService
    {
        //Decided against using delegate so I can pass information around, but I did experiment with it!
        //public delegate void AddMovieEventHandler(object sender, EventArgs e);
        //public event AddMovieEventHandler AddMovie;

        public event EventHandler<MovieTrackerEventArgs> AddMovie;

        public void StartMovieAdd(Movie movie)
        {
            var data = new MovieTrackerEventArgs();
            data.MovieInfo = movie;

            Console.WriteLine($"Currently adding movie '{movie.Title}'");
            Thread.Sleep(2000);

            OnAddMovie(data);
        }

        protected virtual void OnAddMovie(MovieTrackerEventArgs e) {
            if (AddMovie != null)
                AddMovie(this, e);
                      
        }

    }

    class Program 
    {
        static void Main(string[] args) 
        {
            MovieTracker MovieTracker = new MovieTracker();
            Movie movie1 = new Movie(title: "Home Alone", dateAdded: new DateTime(2022, 10, 04, 19, 24, 01), serialNum: 123245113);
            Movie movie2 = new Movie(title: "The Godfather", dateAdded: new DateTime(2022, 10, 04, 07, 04, 21), serialNum: 388102801);
            Movie movie3 = new Movie(title: "Inception", dateAdded: new DateTime(2022, 10, 04, 11, 54, 29), serialNum: 132423332);
            Movie movie4 = new Movie(title: "Spirited Away", dateAdded: new DateTime(2022, 10, 04, 05, 06, 43), serialNum: 135912383);
            Movie movie5 = new Movie(title: "Parasite", dateAdded: new DateTime(2022, 10, 04, 08, 28, 32), serialNum: 564545475);

            Movie[] movieCollection = {movie1, movie2, movie3, movie4, movie5};

            MovieTrackerService MovieTrackerService = new MovieTrackerService();    
            MovieTrackerService.AddMovie += MovieTracker.OnAddMovie;
            
            MovieTracker.PrintData();

            foreach (Movie movie in movieCollection) 
                MovieTrackerService.StartMovieAdd(movie);   

            MovieTracker.PrintData();
            
        }
    }
}