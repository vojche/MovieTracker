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
using System.Runtime.InteropServices;

namespace MovieTracker
{

    public partial class Form1 : Form
    {
        public List<SearchMovie> searchList;
        public SearchMovie curr;
        public MovieDetails modalMovie;
        //public static string x { get; set; }
        public bool moreResults = false;
        public bool nextB { get; set; }
        public bool prevB { get; set; }
        private bool internet { get; set; }
        private bool _addWL { get; set; }
        private bool _addW { get; set; }
        private bool _checkBox1Enabled = false;
        DAMovie da;
        List<Movie> WatchlistMovies = new List<Movie>();
        List<Movie> WatchedMovies = new List<Movie>();
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            checkInternetConnection();
            MaximizeBox = false;
            pictureBox2.Image = Bitmap.FromFile(@"..\..\Pictures\logo.png");
            pictureBox4.Image = Bitmap.FromFile(@"..\..\Pictures\omdb.png");
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            searchList = new List<SearchMovie>();
            da = new DAMovie();

            GeneralStatistic();
            RatingStatistic();
        }

        private void GeneralStatistic()
        {
            textBox5.Text = da.CountWatchedMovies().ToString();
            textBox6.Text = da.CountMoviesWatchlist().ToString();
            textBox7.Text = da.CountTimeSpent().ToString();
            textBox8.Text = String.Format("{0:0.00}", da.AverageRating());
        }

        private void RatingStatistic()
        {
            progressBar1.Maximum = progressBar2.Maximum = progressBar3.Maximum = progressBar4.Maximum = progressBar5.Maximum = progressBar6.Maximum = da.CountWatchedMovies();
            progressBar1.Value = da.RatingBellow();
            textBox2.Text = progressBar1.Value.ToString();

            progressBar2.Value = da.RatingBetween(5.5, 6.4);
            textBox9.Text = progressBar2.Value.ToString();

            progressBar3.Value = da.RatingBetween(6.5, 7.4);
            textBox10.Text = progressBar3.Value.ToString();

            progressBar4.Value = da.RatingBetween(7.5, 8.4);
            textBox11.Text = progressBar4.Value.ToString();

            progressBar5.Value = da.RatingBetween(8.5, 9.4);
            textBox12.Text = progressBar5.Value.ToString();

            progressBar6.Value = da.RatingAbove();
            textBox13.Text = progressBar6.Value.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            /*_addW = addW.Enabled;
            _addWL = addWL.Enabled;*/
            checkInternetConnection();
        }

        [DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int description, int reservedValue);

        public static bool IsInternetAvailable()
        {
            int description;
            return InternetGetConnectedState(out description, 0);
        }

        private void additionButtons()
        {
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
                        _addW = addW.Enabled = true;
                        _addWL = addWL.Enabled = true;
                    }
                    else if (type.First() == 1)
                    {
                        _addW = addW.Enabled = true;
                        _addWL = addWL.Enabled = false;
                    }
                    else if (type.First() == 2)
                    {
                        _addW = addW.Enabled = false;
                        _addWL = addWL.Enabled = false;
                    }
                }
                else if (IsAdded == 0)
                {
                    _addW = addW.Enabled = true;
                    _addWL = addWL.Enabled = true;
                }
            }
        }

        private void checkInternetConnection()
        {
            internet = IsInternetAvailable();
           
            if (internet)
            {                
                internetLabel.Text = "Internet connection: UP";
                addW.Enabled = _addW;
                addWL.Enabled = _addWL;
                checkBox1.Enabled = _checkBox1Enabled;            
            }
            else
            {
                internetLabel.Text = "Internet connection: DOWN";
                checkBox1.Checked = checkBox1.Enabled = addWL.Enabled = addW.Enabled = internet;
            }
            button1.Enabled = textBox4.Enabled =  listBox1.Enabled = internet;

            if (textBox1.Text.Length != 0)
                details.Enabled = internet;

            /*if (textBox4.Text.Length != 0)
                checkBox1.Enabled = internet;*/

            textBox4.ReadOnly =  !internet;
            
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

                _checkBox1Enabled = checkBox1.Enabled = true;
            }
            else
            {
                MessageBox.Show("Movie \"" + textBox4.Text + "\" doesn`t exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _checkBox1Enabled = checkBox1.Enabled = false;
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
                released = new DateTime(1800, 01, 01);
            }    

            List<string> genres = new List<string>();

            string[] gen2 = o["Genre"].ToString().Split(',');
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
            modalMovie = new MovieDetails(o["Title"].ToString(), (int)o["Year"], o["imdbID"].ToString(), o["Poster"].ToString(), released, runtime, genres, o["Director"].ToString(), o["Actors"].ToString(), o["Plot"].ToString(), o["Language"].ToString(), o["Awards"].ToString(), imdbRating);

            runtime = 3;
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
                curr.postaviPoster(pictureBox1, internet);
                details.Enabled = true;

                additionButtons();
            }

            
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void details_Click(object sender, EventArgs e)
        {
            povleciDetalniPodatoci(curr.imdbID);
            Modal modal = new MovieTracker.Modal(modalMovie, addWL.Enabled, addW.Enabled, internet);
                        
            if(modal.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
            {
                if(_addW==true && modal.watchedButton == false)
                {
                    _addW = addW.Enabled = _addWL = addWL.Enabled = false;
                    GeneralStatistic();
                    RatingStatistic();
                }
                else if(_addWL==true && modal.watchlistButton == false)
                {
                    _addWL = addWL.Enabled = false;
                    textBox6.Text = da.CountMoviesWatchlist().ToString();
                }
            }           
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
                int page = 0;
                int.TryParse(pageNumber.Text, out page);
                if (page > 1)
                    prev.Enabled = true;
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
            _checkBox1Enabled = moreResults = checkBox1.Checked = checkBox1.Enabled = false;
        }

        private void addWL_Click(object sender, EventArgs e)
        {
            povleciDetalniPodatoci(curr.imdbID);
            using (var ctx = new MovieContext())
            {
                var movie = new Movie
                {
                    ImdbID = modalMovie.imdbID,
                    Title = modalMovie.title,
                    Year = modalMovie.year,
                    Release = modalMovie.release,
                    Runtime = modalMovie.runtime,
                    Director = modalMovie.director,
                    Actors = modalMovie.actors,
                    Plot = modalMovie.plot,
                    Language = modalMovie.language,
                    Awards = modalMovie.awards,
                    Image = modalMovie.poster,
                    Rating = (decimal)modalMovie.imdbRating,
                    Type = 1
                };

                foreach (string genre in modalMovie.genres)
                {
                    if (ctx.Genres.Any(m => m.Name == genre) == false)
                    {
                        Genre g = new Genre { Name = genre };
                        ctx.Genres.Add(g);
                        movie.Genres.Add(g);
                    }
                    else
                    {
                        var obj = ctx.Genres.First(m => m.Name == genre);
                        movie.Genres.Add(obj);
                    }
                }
                ctx.Movies.Add(movie);
                ctx.SaveChanges();
            }
            _addWL = addWL.Enabled = false;
            textBox6.Text = da.CountMoviesWatchlist().ToString();
            // za vo tabot watch list
            /*SearchMovie selectedMovie = listBox1.SelectedItem as SearchMovie;
            toWatchList.Items.Add(selectedMovie);
            numMoviesToWatch.Text = toWatchList.Items.Count + "";*/
        }

        private void addW_Click(object sender, EventArgs e)
        {
			povleciDetalniPodatoci(curr.imdbID);
            using (var ctx = new MovieContext())
            {
                var type = ctx.Movies.Where(m => m.ImdbID == modalMovie.imdbID).Select(m => m.Type).SingleOrDefault();

                if (type == null)
                {
                    var movie = new Movie
                    {
                        ImdbID = modalMovie.imdbID,
                        Title = modalMovie.title,
                        Year = modalMovie.year,
                        Release = modalMovie.release,
                        Runtime = modalMovie.runtime,
                        Director = modalMovie.director,
                        Actors = modalMovie.actors,
                        Plot = modalMovie.plot,
                        Language = modalMovie.language,
                        Awards = modalMovie.awards,
                        Image = modalMovie.poster,
                        Rating = (decimal)modalMovie.imdbRating,
                        Type = 2
                    };

                    foreach (string genre in modalMovie.genres)
                    {
                        Genre g = new Genre { Name = genre };
                        if (ctx.Genres.Any(m => m.Name == genre) == false)
                        {
                            ctx.Genres.Add(g);
                            movie.Genres.Add(g);
                        }
                        else
                        {
                            var obj = ctx.Genres.First(m => m.Name == genre);
                            movie.Genres.Add(obj);
                        }
                    }
                    ctx.Movies.Add(movie);
                    ctx.SaveChanges();
                }
                else if (type == 1)
                {
                    var test = ctx.Movies.Single(m => m.ImdbID == modalMovie.imdbID);
                    test.Type = 2;
                    ctx.SaveChanges();
                }

            }
            
            _addW = addW.Enabled = false;
            _addWL = addWL.Enabled = false;
            GeneralStatistic();

            RatingStatistic();
        }

        private List<Movie> SortingFiltring(List<Movie> list)
        {
            List<Movie> ret = new List<Movie>();
            if(radioButton4.Checked == true || radioButton1.Checked == true)
            {
                ret = list.OrderByDescending(x => x.Year).ToList();
            }
            else if(radioButton2.Checked == true || radioButton3.Checked == true)
            {
                ret = list.OrderByDescending(x => x.Rating).ToList();
            }

            return ret;
        }

        private void watchedList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (watchedList.SelectedIndex != -1)
            {
                Movie selected = watchedList.SelectedItem as Movie;
                textBox25.Text = selected.Title;
                textBox24.Text = selected.RuntimeMethod();
                textBox32.Text = selected.ReleaseMethod();
                textBox27.Text = selected.RatingMethod();
                textBox23.Text = selected.Awards;
                textBox31.Text = selected.GenresMethod();
                textBox30.Text = selected.Director;
                textBox29.Text = selected.Actors;
                textBox28.Text = selected.Language;
                textBox26.Text = selected.Plot;
                selected.setPoster(pictureBox5, internet);
                button5.Enabled = true;
            }
        }
                
        private void toWatchList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(toWatchList.SelectedIndex != -1)
            {
                Movie selected = toWatchList.SelectedItem as Movie;
                textBox15.Text = selected.Title;
                textBox14.Text = selected.RuntimeMethod();
                textBox22.Text = selected.ReleaseMethod();
                textBox17.Text = selected.RatingMethod();
                textBox3.Text = selected.Awards;
                textBox21.Text = selected.GenresMethod();
                textBox20.Text = selected.Director;
                textBox19.Text = selected.Actors;
                textBox18.Text = selected.Language;
                textBox16.Text = selected.Plot;
                selected.setPoster(pictureBox3, internet);
                button2.Enabled = button3.Enabled = true;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                listBox1.ClearSelected();
                textBox1.Clear();
                pictureBox1.Image = null;
                details.Enabled = false;
                addW.Enabled = false;
                addWL.Enabled = false;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                button5.Enabled = false;
                pictureBox5.Image = null;
                textBox23.Clear();
                textBox24.Clear();
                textBox25.Clear();
                textBox26.Clear();
                textBox27.Clear();
                textBox28.Clear();
                textBox29.Clear();
                textBox30.Clear();
                textBox31.Clear();
                textBox32.Clear();

                WatchedMovies.Clear();
                WatchedMovies = da.ReturnList(2);
                watchedList.Items.Clear();
                radioButton3.Checked = radioButton4.Checked = false;
                if (WatchedMovies.Count != 0)
                {
                    foreach (Movie m in WatchedMovies)
                    {
                        watchedList.Items.Add(m);
                    }
                    radioButton4.Enabled = radioButton3.Enabled = comboBox2.Enabled = true;
                }                

            }
            else if (tabControl1.SelectedIndex == 2)
            {
                button2.Enabled = button3.Enabled = false;
                pictureBox3.Image = null;
                textBox3.Clear();
                textBox14.Clear();
                textBox15.Clear();
                textBox16.Clear();
                textBox17.Clear();
                textBox18.Clear();
                textBox19.Clear();
                textBox20.Clear();
                textBox21.Clear();
                textBox22.Clear();

                WatchlistMovies.Clear();
                WatchlistMovies = da.ReturnList(1);
                toWatchList.Items.Clear();
                radioButton1.Checked = radioButton2.Checked = false;
                if (WatchlistMovies.Count != 0)
                {
                    foreach (Movie m in WatchlistMovies)
                    {
                        toWatchList.Items.Add(m);
                    }
                    radioButton1.Enabled = radioButton2.Enabled = comboBox1.Enabled = true;
                }               
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            watchedList.Items.Clear();
            foreach (Movie m in SortingFiltring(WatchedMovies))
            {
                watchedList.Items.Add(m);
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            watchedList.Items.Clear();
            foreach (Movie m in SortingFiltring(WatchedMovies))
            {
                watchedList.Items.Add(m);
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            toWatchList.Items.Clear();
            foreach (Movie m in SortingFiltring(WatchlistMovies))
            {
                toWatchList.Items.Add(m);
            }

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            toWatchList.Items.Clear();
            foreach (Movie m in SortingFiltring(WatchlistMovies))
            {
                toWatchList.Items.Add(m);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Movie selected = watchedList.SelectedItem as Movie;
            da.UpdateStatus(selected, null);

            button5.Enabled = false;
            pictureBox5.Image = null;
            textBox23.Clear();
            textBox24.Clear();
            textBox25.Clear();
            textBox26.Clear();
            textBox27.Clear();
            textBox28.Clear();
            textBox29.Clear();
            textBox30.Clear();
            textBox31.Clear();
            textBox32.Clear();

            WatchedMovies.Clear();
            WatchedMovies = da.ReturnList(2);
            watchedList.Items.Clear();
            radioButton3.Checked = radioButton4.Checked = false;
            if (WatchedMovies.Count != 0)
            {
                foreach (Movie m in WatchedMovies)
                {
                    watchedList.Items.Add(m);
                }
                radioButton4.Enabled = radioButton3.Enabled = comboBox2.Enabled = true;
            }
        }
    }
}
