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

        public string CountTimeSpent()
        {
            ctx = new MovieContext();
            var time = ctx.Movies.Where(m => m.Type == 2).Select(m => m.Runtime).DefaultIfEmpty(0).Sum();
            ctx.Dispose();
            TimeSpan ts = TimeSpan.FromMinutes(time);
            int months;
            int days;
            int hours = ts.Hours;
            int minutes = ts.Minutes;
            if (ts.Days >= 30)
            {
                months = ts.Days / 30;
                days = ts.Days % 30;
            }
            else
            {
                days = ts.Days;
                months = 0;
            }

            if (months == 0 && days == 0 && hours == 0 && minutes == 0)
            {
                return String.Format("You have not watched movies yet!");
            }
            else if (months == 0 && days == 0 && hours == 0)
            {
                return String.Format("{0} minutes", minutes);
            }
            else if (months == 0 && days == 0)
            {
                return String.Format("{0} hours, {1} minutes", hours, minutes);
            }
            else if (months == 0)
            {
                return String.Format("{0} days, {1} hours, {2} minutes", days, hours, minutes);
            }
            else
            {
                return String.Format("{0} months, {1} days, {2} hours, {3} minutes", months, days, hours, minutes);
            }
        }

        public int RatingBetween(double min, double max)
        {
            ctx = new MovieContext();
            var count = ctx.Movies.Where(m => m.Type == 2).Where(m => (double)m.Rating >= min && (double)m.Rating <= max).Count();
            ctx.Dispose();
            return count;
        }

        public int RatingBellow()
        {
            ctx = new MovieContext();
            var count = ctx.Movies.Where(m => m.Type == 2).Where(m => (double)m.Rating <= 5.4).Count();
            ctx.Dispose();
            return count;
        }

        public int RatingAbove()
        {
            ctx = new MovieContext();
            var count = ctx.Movies.Where(m => m.Type == 2).Where(m => (double)m.Rating >= 9.5).Count();
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

        public List<Movie> ReturnList(int type)
        {
            ctx = new MovieContext();
            var list = ctx.Movies.Where(m => m.Type == type).ToList();
            ctx.Dispose();
            return list;
        }

        public void UpdateStatus(Movie movie, int? status)
        {
            ctx = new MovieContext();
            var mov = ctx.Movies.Where(m => m.ImdbID == movie.ImdbID).FirstOrDefault();
            mov.Type = status;
            ctx.SaveChanges();
            ctx.Dispose();
        }
               
    }
}
