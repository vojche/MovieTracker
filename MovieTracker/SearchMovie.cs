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

        public void postaviPoster(PictureBox pictureBox, bool internet)
        {
            //string link;
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

        public override string ToString()
        {
            return title + " - " + year.ToString();
            //return base.ToString();
        }
    }
}
