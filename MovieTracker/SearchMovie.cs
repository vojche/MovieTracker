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

        public SearchMovie(string title, int year, string imdbID, string poster)
        {
            this.title = title;
            this.year = year;
            this.imdbID = imdbID;
            this.poster = poster;
        }

        public void postaviPoster(PictureBox pictureBox)
        {
            string link;
            if (!poster.Equals("N/A"))
            {
                link = poster;
            }
            else
            {
                link = "https://cdn4.iconfinder.com/data/icons/project-document-std-pack-4/512/trailer-512.png";
            }

            var request = WebRequest.Create(link);
            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                pictureBox.Image = Bitmap.FromStream(stream);
            }
        }

        public override string ToString()
        {
            return title + " - " + year.ToString();
            //return base.ToString();
        }
    }
}
