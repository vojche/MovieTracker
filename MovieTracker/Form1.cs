using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using MovieTracker.DAL;
using System.Net;
using System.IO;

namespace MovieTracker
{

    public partial class Form1 : Form
    {
        public List<SearchMovie> searchList;
        public SearchMovie curr;
        public MovieDetails modalMovie;
        public static string x { get; set; }
        public bool moreResults = false;
        public bool nextB { get; set; }
        public bool prevB { get; set; }
        DAMovie da;
        public Form1()
        {
            InitializeComponent();
            searchList = new List<SearchMovie>();
            da = new DAMovie();
            textBox5.Text =  da.CountWatchedMovies().ToString();
            textBox6.Text = da.CountMoviesWatchlist().ToString();
            textBox7.Text = da.CountTimeSpent().ToString();
        }

        private void povleciSearchPodatoci(string ime, int page)
        {
            searchList.Clear();

            WebClient c = new WebClient();
            string data = c.DownloadString("http://www.omdbapi.com/?s=" + ime.Trim() + "&type=movie&page=" + page);
            JObject o = JObject.Parse(data);

            if (o["Response"].ToString().Equals("True"))
            {
                JArray niza = (JArray)o["Search"];

                foreach (JObject pom in niza)
                {
                    searchList.Add(new SearchMovie(pom["Title"].ToString(), (int)pom["Year"], pom["imdbID"].ToString(), pom["Poster"].ToString()));
                }

                searchList = searchList.OrderBy(x => x.year).ToList();

                int total = (int)o["totalResults"];
                total = (int)Math.Ceiling((decimal)total / 10);
                int pageNum = 0;
                int.TryParse(pageNumber.Text, out pageNum);                
                    
                if (total == pageNum)
                    nextB = false;
                else nextB = true;

                if (moreResults)
                    next.Enabled = nextB;

                checkBox1.Enabled = true;
            }
            else
            {
                MessageBox.Show("Movie \"" + textBox4.Text + "\" doesn`t exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                checkBox1.Enabled = false;
            }
        }

        private int mesec(string mesec)
        {
            int x = 0;
            switch (mesec)
            {
                case "Jan":
                    x = 1;
                    break;
                case "Feb":
                    x = 2;
                    break;
                case "Mar":
                    x = 3;
                    break;
                case "Apr":
                    x = 4;
                    break;
                case "May":
                    x = 5;
                    break;
                case "Jun":
                    x = 6;
                    break;
                case "Jul":
                    x = 7;
                    break;
                case "Aug":
                    x = 8;
                    break;
                case "Sep":
                    x = 9;
                    break;
                case "Oct":
                    x = 10;
                    break;
                case "Nov":
                    x = 11;
                    break;
                case "Dec":
                    x = 12;
                    break;
            }

            return x;

        }

        private void povleciDetalniPodatoci(string id)
        {
            WebClient c = new WebClient();
            string data = c.DownloadString("http://www.omdbapi.com/?i=" + id + "&plot=full&r=json");
            JObject o = JObject.Parse(data);
            DateTime released;
            if (!o["Released"].ToString().Equals("N/A"))
            {
                string date = o["Released"].ToString();
                string[] d = date.Split(' ');
                int godina = 0;
                int.TryParse(d[2], out godina);
                int den = 0;
                int.TryParse(d[0], out den);

                released = new DateTime(godina, mesec(d[1]), den);
            }
            else
            {
                released = new DateTime(1550, 01, 01);
            }    

            List<string> genres = new List<string>();
            //string gen = o["Genre"].ToString();
            string[] gen2 = o["Genre"].ToString().Split(' ');
            foreach (string i in gen2)
            {
                genres.Add(i.Trim());
            }

            string[] runtimeA = o["Runtime"].ToString().Split(' ');
            int runtime = 0;
            int.TryParse(runtimeA[0], out runtime);
            float imdbRating = 0;
            if (!o["imdbRating"].ToString().Equals("N/A"))
            {
                imdbRating = (float)o["imdbRating"];
            }
            modalMovie = new MovieDetails(o["Title"].ToString(), (int)o["Year"], o["imdbID"].ToString(), o["Poster"].ToString(), released, runtime, genres, o["Director"].ToString(), o["Actors"].ToString(), o["Plot"].ToString(), o["Language"].ToString(), o["Language"].ToString(), imdbRating);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox4.Text.Trim().Length != 0)
            {
                
                pageNumber.Text = "1";
                povleciSearchPodatoci(textBox4.Text, 1);                
                moreResults = checkBox1.Checked = false;
                listBox1.Items.Clear();
                foreach (SearchMovie film in searchList)
                {
                    listBox1.Items.Add(film);
                }                
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                curr = listBox1.SelectedItem as SearchMovie;
                textBox1.Text = curr.title;
                curr.postaviPoster(pictureBox1);
                details.Enabled = true;

                using (var ctx = new MovieContext())
                {
                    int IsAdded = ctx.Movies.Where(m => m.ImdbID == curr.imdbID).Count();

                    if (IsAdded == 1)
                    {
                        var type = from movie in ctx.Movies
                                   where movie.ImdbID == curr.imdbID
                                   select movie.Type;

                        if (type.First() == null)
                        {
                            addW.Enabled = true;
                            addWL.Enabled = true;
                        }
                        else if (type.First() == 1)
                        {
                            addW.Enabled = true;
                            addWL.Enabled = false;
                        }
                        else if (type.First() == 2)
                        {
                            addW.Enabled = false;
                            addWL.Enabled = true;
                        }
                    }
                    else if (IsAdded == 0)
                    {
                        addW.Enabled = true;
                        addWL.Enabled = true;
                    }
                }
            }

            /*if (!film.poster.Equals("N/A"))
            {
                var request = WebRequest.Create(film.poster);

                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    pictureBox1.Image = Bitmap.FromStream(stream);
                }
            }
            else
            {
                var request = WebRequest.Create("https://upload.wikimedia.org/wikipedia/commons/thumb/9/97/Dialog-error-round.svg/2000px-Dialog-error-round.svg.png");

                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    pictureBox1.Image = Bitmap.FromStream(stream);
                }
            }*/
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void details_Click(object sender, EventArgs e)
        {
            povleciDetalniPodatoci(curr.imdbID);
            Modal modal = new MovieTracker.Modal(modalMovie, true, false);
            modal.ShowDialog();
        }
                
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (moreResults)
            {
                moreResults = false;
                prev.Enabled = next.Enabled = moreResults;               
            }
            else
            {
                moreResults = true;
                next.Enabled = nextB;
            }
            
        }

        private void next_Click(object sender, EventArgs e)
        {
           
                int page = 0;
                int.TryParse(pageNumber.Text, out page);
                page += 1;
                pageNumber.Text = page.ToString();
                povleciSearchPodatoci(textBox4.Text, page);
                listBox1.Items.Clear();
                foreach (SearchMovie film in searchList)
                {
                    listBox1.Items.Add(film);
                }
                prev.Enabled = true;

            
        }

        private void prev_Click(object sender, EventArgs e)
        {
            
                int page = 0;
                int.TryParse(pageNumber.Text, out page);
                page -= 1;
                pageNumber.Text = page.ToString();
                povleciSearchPodatoci(textBox4.Text, page);
                listBox1.Items.Clear();
                foreach (SearchMovie film in searchList)
                {
                    listBox1.Items.Add(film);
                }

                if (page == 1)
                    prev.Enabled = false;
                else prev.Enabled = true;

                next.Enabled = true;
            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {            
            moreResults = checkBox1.Checked = checkBox1.Enabled = false;
        }

        private void addWL_Click(object sender, EventArgs e)
        {
            povleciDetalniPodatoci(curr.imdbID);
            using (var ctx = new MovieContext())
            {
                var query = ctx.Movies.Where(m => m.ImdbID == modalMovie.imdbID).Count();

                var movie = new Movie { ImdbID = modalMovie.imdbID ,Title = modalMovie.title,
                    Year = modalMovie.release, Runtime = modalMovie.runtime, Director = modalMovie.director,
                    Actors = modalMovie.actors, Plot = modalMovie.plot, Language = modalMovie.language, Awards = modalMovie.awards,
                    Image = modalMovie.poster, Rating = (decimal)modalMovie.imdbRating, Type = 1 };
                foreach (string genre in modalMovie.genres)
                {
                    Genre g = new Genre { Name = genre };
                    ctx.Genres.Add(g);
                }
                ctx.Movies.Add(movie);
                ctx.SaveChanges();
            }
            addWL.Enabled = false;
        }
    }
}
