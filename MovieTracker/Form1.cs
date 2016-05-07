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
        public Form1()
        {
            InitializeComponent();
            searchList = new List<SearchMovie>();
        }

        private void povleciSearchPodatoci(string ime)
        {
            searchList.Clear();

            WebClient c = new WebClient();
            string data = c.DownloadString("http://www.omdbapi.com/?s=" + ime.Trim() + "&type=movie");
            JObject o = JObject.Parse(data);

            if (o["Response"].ToString().Equals("True"))
            {
                JArray niza = (JArray)o["Search"];

                foreach (JObject pom in niza)
                {
                    searchList.Add(new SearchMovie(pom["Title"].ToString(), (int)pom["Year"], pom["imdbID"].ToString(), pom["Poster"].ToString()));
                }

                searchList = searchList.OrderBy(x => x.year).ToList();
            }
            else
            {
                 MessageBox.Show("Movie \"" + textBox4.Text + "\" doesn`t exist!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            var data = c.DownloadString("http://www.omdbapi.com/?i=" + id + "&plot=full&r=json");
            JObject o = JObject.Parse(data);
            string date = o["Released"].ToString();
            string[] d = date.Split(' ');
            int godina = 0;
            int.TryParse(d[2], out godina);
            int den = 0;
            int.TryParse(d[0], out den);

            List<string> actors = new List<string>();
            //string ac = o["Actors"].ToString();
            string[] ac2 = o["Actors"].ToString().Split(',');
            foreach (string i in ac2)
            {
                actors.Add(i.Trim());
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

            modalMovie = new MovieDetails(o["Title"].ToString(), (int)o["Year"], o["imdbID"].ToString(), o["Poster"].ToString(), new DateTime(godina, mesec(d[1]), den), runtime, genres, o["Director"].ToString(), actors, o["Plot"].ToString(), o["Language"].ToString(), (float)o["imdbRating"]);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox4.Text.Trim().Length != 0)
            {
                povleciSearchPodatoci(textBox4.Text);
                listBox1.Items.Clear();
                foreach (SearchMovie film in searchList)
                {
                    listBox1.Items.Add(film);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listBox1.SelectedIndex != -1)
            {
                curr = listBox1.SelectedItem as SearchMovie;
                textBox1.Text = curr.title;
                textBox2.Text = curr.year.ToString();
                curr.postaviPoster(pictureBox1);
                addW.Enabled = true;
                addWL.Enabled = true;
                details.Enabled = true;

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
    }
}
