using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieTracker
{
    class User
    {
        private readonly ObservableListSource<Movie> watched =
                new ObservableListSource<Movie>();

        private readonly ObservableListSource<Movie> watchlist =
                new ObservableListSource<Movie>();

        public int userID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public virtual ObservableListSource<Movie> WatchedMovies { get { return watched; } }
        public virtual ObservableListSource<Movie> Watchlist { get { return watchlist; } }
    }
}
