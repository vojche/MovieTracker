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
        public MovieDetails movie;
        public bool watchlistButton { get; set; }
        public bool watchedButton { get; set; }
        public bool internet { get; set; }
        public Modal(MovieDetails movie, bool watchlistButton, bool watchedButton, bool internet)
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

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox10_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
