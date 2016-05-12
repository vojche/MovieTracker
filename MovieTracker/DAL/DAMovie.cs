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

        public double AverageRating()
        {
            ctx = new MovieContext();
            var rating = ctx.Movies.Where(m => m.Type == 2).Select(m => (double?)m.Rating).Average();
            ctx.Dispose();
            if (rating == null)
            {
                return 0;
            }
            return (double)rating;
        }

        public List<Movie> WatchlistMovies()
        {
            ctx = new MovieContext();
            var list = ctx.Movies.Where(m => m.Type == 1).ToList();
            ctx.Dispose();
            return list;
        }

        public List<Movie> WatchedMovies()
        {
            ctx = new MovieContext();
            var list = ctx.Movies.Where(m => m.Type == 2).ToList();
            ctx.Dispose();
            return list;
        }


    }
}
