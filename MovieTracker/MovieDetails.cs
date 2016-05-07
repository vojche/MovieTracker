using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker
{
    public class MovieDetails : SearchMovie
    {
        public DateTime release;
        public int runtime { get; set; }
        public List<string> genres;
        public string director { get; set; }
        public List<string> actors;
        public string plot { get; set; }
        public string language { get; set; }
        public float imdbRating { get; set; }


        public MovieDetails(string title, int year, string imdbID, string poster, DateTime release, int runtime, List<string> genres, string director, List<string> actors, string plot, string language, float imdbRating) : base(title, year, imdbID, poster)
        {
            this.release = release;
            this.runtime = runtime;
            this.genres = genres;
            this.director = director;
            this.actors = actors;
            this.plot = plot;
            this.language = language;
            this.imdbRating = imdbRating;

        }

        public string Release()
        {
            return release.Month + "." + release.Day + "." + release.Year;
        }

        public string Actors()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in actors)
            {
                sb.Append(s + " ");
            }
            return sb.ToString();
        }

        public string Genres()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in genres)
            {
                sb.Append(s + " ");
            }
            return sb.ToString();
        }
    }
}
