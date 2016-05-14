using MovieTracker.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MovieTracker
{
    public partial class Modal : Form
    {
        public SearchMovie movie;
        public bool watchlistButton { get; set; }
        public bool watchedButton { get; set; }
        public bool internet { get; set; }
        DAMovie da;
        public Modal(SearchMovie movie, bool watchlistButton, bool watchedButton, bool internet)
        {
            InitializeComponent();
            MaximizeBox = false;            
            this.FormBorderStyle = FormBorderStyle.FixedSingle;

            this.movie = movie;
            this.watchlistButton = watchlistButton;
            this.watchedButton = watchedButton;
            movie.postaviPoster(pictureBox1, internet);
            textBox1.Text = movie.title;
            textBox2.Text = movie.Runtime();
            textBox3.Text = movie.Release();
            textBox4.Text = movie.Genres();
            textBox5.Text = movie.director;
            textBox6.Text = movie.actors;
            textBox7.Text = movie.language;
            textBox8.Text = movie.Rating();
            textBox9.Text = movie.plot;
            textBox10.Text = movie.awards;
            addWL.Enabled = watchlistButton;
            addW.Enabled = watchedButton;
            da = new DAMovie();
        }

        private void addWL_Click(object sender, EventArgs e)
        {
            using (var ctx = new MovieContext())
            {
                var type = ctx.Movies.Where(m => m.ImdbID == movie.imdbID).Select(m => m.Type).SingleOrDefault();

                if (type == null)
                {
                    var movie1 = new Movie
                    {
                        ImdbID = movie.imdbID,
                        Title = movie.title,
                        Year = movie.year,
                        Release = movie.release,
                        Runtime = movie.runtime,
                        Director = movie.director,
                        Actors = movie.actors,
                        Plot = movie.plot,
                        Language = movie.language,
                        Awards = movie.awards,
                        Image = movie.poster,
                        Rating = (decimal)movie.imdbRating,
                        Type = 1
                    };

                    foreach (string genre in movie.genres)
                    {
                        if (ctx.Genres.Any(m => m.Name == genre) == false)
                        {
                            Genre g = new Genre { Name = genre };
                            ctx.Genres.Add(g);
                            movie1.Genres.Add(g);
                        }
                        else
                        {
                            var obj = ctx.Genres.First(m => m.Name == genre);
                            movie1.Genres.Add(obj);
                        }
                    }

                    ctx.Movies.Add(movie1);
                    ctx.SaveChanges();
                }
                else if (type == 0)
                {
                    var movie1 = ctx.Movies.Where(m => m.ImdbID == movie.imdbID).SingleOrDefault();
                    da.UpdateStatus(movie1, 1);
                }
            }
            watchlistButton = addWL.Enabled = false;
        }

        private void addW_Click(object sender, EventArgs e)
        {
            
            using (var ctx = new MovieContext())
            {
                var type = ctx.Movies.Where(m => m.ImdbID == movie.imdbID).Select(m => m.Type).SingleOrDefault();

                if (type == null)
                {
                    var movie1 = new Movie
                    {
                        ImdbID = movie.imdbID,
                        Title = movie.title,
                        Year = movie.year,
                        Release = movie.release,
                        Runtime = movie.runtime,
                        Director = movie.director,
                        Actors = movie.actors,
                        Plot = movie.plot,
                        Language = movie.language,
                        Awards = movie.awards,
                        Image = movie.poster,
                        Rating = (decimal)movie.imdbRating,
                        Type = 2
                    };

                    foreach (string genre in movie.genres)
                    {
                        Genre g = new Genre { Name = genre };
                        if (ctx.Genres.Any(m => m.Name == genre) == false)
                        {
                            ctx.Genres.Add(g);
                            movie1.Genres.Add(g);
                        }
                        else
                        {
                            var obj = ctx.Genres.First(m => m.Name == genre);
                            movie1.Genres.Add(obj);
                        }
                    }
                    ctx.Movies.Add(movie1);
                    ctx.SaveChanges();
                }
                else if (type == 0)
                {
                    var movie1 = ctx.Movies.Where(m => m.ImdbID == movie.imdbID).SingleOrDefault();
                    da.UpdateStatus(movie1, 2);
                }
                else if (type == 1)
                {
                    var test = ctx.Movies.Single(m => m.ImdbID == movie.imdbID);
                    test.Type = 2;
                    ctx.SaveChanges();
                }

            }

            watchedButton = addW.Enabled = false;
            watchlistButton = addWL.Enabled = false;
            
        }
    }
}
