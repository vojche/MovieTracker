using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieTracker
{
    public class SearchMovie
    {
        public string title { get; set; }
        public int year { get; set; }
        public string imdbID { get; set; }
        public string poster { get; set; }

        public DateTime release
        {
            get; set;
        }
        public int runtime { get; set; }
        public List<string> genres { get; set; }
        public string director { get; set; }
        public string actors { get; set; }
        public string plot { get; set; }
        public string language { get; set; }
        public string awards { get; set; }
        public float imdbRating { get; set; }

        public SearchMovie(string title, int year, string imdbID, string poster)
        {
            this.title = title;
            this.year = year;
            this.imdbID = imdbID;
            this.poster = poster;

            release = new DateTime();
            genres = new List<string>();
        }

        public void postaviPoster(PictureBox pictureBox, bool internet)
        {           
            try
            { 
                if (internet)
                {
                    if (!poster.Equals("N/A"))
                    {
                        var request = WebRequest.Create(poster);
                        using (var response = request.GetResponse())
                        using (var stream = response.GetResponseStream())
                        {
                            pictureBox.Image = Bitmap.FromStream(stream);
                        }
                    }
                    else
                    {
                        pictureBox.Image = Bitmap.FromFile(@"..\..\Pictures\default.png");
                    }
                }
                else
                {
                    pictureBox.Image = Bitmap.FromFile(@"..\..\Pictures\default.png");
                }
            }
            catch(WebException we)
            {
                pictureBox.Image = Bitmap.FromFile(@"..\..\Pictures\default.png");
            }
}

        public override string ToString()
        {
            return title + " - " + year.ToString();
            //return base.ToString();
        }

        public string Rating()
        {
            if (imdbRating == 0)
                return "N/A";
            else return string.Format("{0:0.0}", imdbRating); //.ToString();
        }
        
        public string Runtime()
        {
            if (runtime == 0)
                return "N/A";
            else return runtime.ToString() + " min";
        }

        public string Release()
        {
            if (release.Year != 1800)
                return release.Month + "." + release.Day + "." + release.Year;
            else
            {
                return "N/A";
            }
        }

        public string Genres()
        {
            StringBuilder sb = new StringBuilder();
            string last = genres.Last();
            foreach (string s in genres)
            {
                if (s.Equals(last))
                {
                    sb.Append(s);
                }
                else
                {
                    sb.Append(s + ", ");
                }
            }
            return sb.ToString();
        }

    }
}
