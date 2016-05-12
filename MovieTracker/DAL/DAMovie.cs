using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker.DAL
{
    class DAMovie
    {
        MovieContext ctx;

        public int CountWatchedMovies()
        {
            ctx = new MovieContext();
            var counter = ctx.Movies.Where(m => m.Type == 2).Count();
            ctx.Dispose();
            return counter;
        }

        public int CountMoviesWatchlist()
        {
            ctx = new MovieContext();
            var counter = ctx.Movies.Where(m => m.Type == 1).Count();
            ctx.Dispose();
            return counter;
        }

        public int CountTimeSpent()
        {
            ctx = new MovieContext();
            var time = ctx.Movies.Where(m => m.Type == 2).Select(m => m.Runtime).DefaultIfEmpty(0).Sum();
            ctx.Dispose();
            return time;
        }

        public int RatingBetween(double min, double max)
        {
            ctx = new MovieContext();
            var count = ctx.Movies.Where(m => m.Type == 2).Select(m => (double)m.Rating >= min && (double)m.Rating <= max).Count();
            ctx.Dispose();
            return count;
        }

        /*public List<MovieDetails> WatchlistMovies()
        {
            List<MovieDetails> wl;
            ctx = new MovieContext();
            List<string> genre = new List<string>();
            genre.Add("N/A");

            wl = ctx.Movies.Where(m => m.Type == 1).Select(row => new MovieDetails {
                title = row.Title,
                year = row.Year,
                imdbID = row.ImdbID,
                poster = row.Image,
                release = row.Year.ToUniversalTime(),
                runtime = row.Runtime,
                genres = genre,
                director = row.Director,
                actors = row.Actors,
                plot = row.Plot,
                language = row.Language,
                awards = row.Awards,
               imdbRating = (double)row.Rating

            }).ToList();

            ctx.Dispose();
            return wl;
        }*/
    }
}
