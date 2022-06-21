using Microsoft.AspNetCore.Mvc;
using MovieLibrary;

namespace BookMyticket.Controllers
{
    public class AdminController : Controller
    {
        CinemaContext dc=new CinemaContext();
        Class1 ob=new Class1();
        public ViewResult home()
        {
            var s = dc.Movies.ToList();
            return View(s);
        }
        [HttpGet]
        public ActionResult Addmovie()
        {
            if (HttpContext.Session.GetString("uname") == null)
            {
                return RedirectToAction("admin");
            }

            else
            {

                return View();
            }

        }

        [HttpPost]
        public ActionResult Addmovie(Movie r)
        {
            int i = ob.Addmovie(r);
            

            if (i > 0)
            {
                ViewData["a"] = "New Movie Added Successfully";
            }
            else
            {
                ViewData["a"] = "Try again";
            }
            return View();
        }

        [HttpGet]
        public ActionResult deletemovie(string myitemname)
        {
            if (HttpContext.Session.GetString("uname") == null)
            {
                return RedirectToAction("admin");
            }

            else
            {
                var result = dc.Movies.ToList().Find(c => c.MovieName == myitemname);
                TempData["n"] = result.MovieName;
                TempData["l"] = result.Movielanguage;
                TempData["d"] = result.MovieDuration;
                TempData.Keep();
                return View(result);
            }

        }
        [HttpPost]
        public ActionResult deletemovie(Movie r, string myitemname)
        {
            var result = dc.Movies.ToList().Find(c => c.MovieName == myitemname);
            dc.Movies.Remove(result);
            dc.SaveChanges();
            ViewData["n"] = "Sucessfully deleted!!";
            return View();

        }
        [HttpGet]
        public ActionResult update(string myitemname)
        {
            if (HttpContext.Session.GetString("uname") == null)
            {
                return RedirectToAction("admin");
            }

            else
            {
                var result = dc.Movies.ToList().Find(c => c.MovieName == myitemname);

                return View(result);
            }

        }

        [HttpPost]
        public ActionResult update(Movie r)
        {
            dc.Movies.Update(r);
            int i = dc.SaveChanges();





            if (i > 0)
            {
                ViewData["a"] = "New Movie updated Successfully";
            }
            else
            {
                ViewData["a"] = "Try again";
            }
            return View();
        }
        [HttpGet]
        public ActionResult admin()
        {

            return View();

        }


        [HttpPost]
        public ActionResult admin(IFormCollection abc)
        {
            string uname = abc["uname"];
            string pwd = abc["pwd"];
            HttpContext.Session.SetString("uname", uname);
            if (uname == "login" && pwd == "1234")
            {
                Response.Redirect("home");
            }
            else
            {
                @ViewData["v"] = "Please enter valid user ID and Password";

            }
            return View("admin");
        }
        [HttpGet]
        public ActionResult checkseats()
        {
            if (HttpContext.Session.GetString("uname") == null)
            {
                return RedirectToAction("admin");
            }

            else
            {
                var getbooking = dc.Bookings.ToList().OrderByDescending(a => a.MovieDate);
                return View(getbooking);
            }

        }
        [HttpGet]
        public ActionResult Getuserdetails()
        {
            if (HttpContext.Session.GetString("uname") == null)
            {
                return RedirectToAction("admin");
            }

            else
            {
                var getusertable = dc.Usertables.ToList();
                return View(getusertable);
            }
        }
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("uname");
            return View();
        }

    }
}
