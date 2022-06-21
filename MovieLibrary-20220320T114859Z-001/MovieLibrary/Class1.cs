namespace MovieLibrary
{
    public class Class1
    {
        CinemaContext dc=new CinemaContext();
        public int Addmovie(Movie a)
        {// logic for contact page goes here

            dc.Movies.Add(a);
            int i = dc.SaveChanges();
            return i;

        }
        public List<Movie> displaySearch(string tofind)
        {

            List<Movie> res = (from t in dc.Movies
                               where t.MovieName.Contains(tofind)
                               select t).ToList();
            return res;
        }
        public int contact(Contactu a)
        {// logic for contact page goes here
            dc.Contactus.Add(a);
            int i = dc.SaveChanges();
            return i;

        }

    }
}