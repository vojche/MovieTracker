using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker
{
    class Movie
    {
        public int id { get; set; }
        public int imdbID { get; set; }
        public string title { get; set; }
        public int year { get; set; }
        public int runtime { get; set; }
        public List<string> genres { get; set; }
        public string director { get; set; }
        public List<string> actors { get; set; }
        public string plot { get; set; }
        public string awards { get; set; }
        public string image { get; set;}
        public double rating { get; set; }
    }
}
