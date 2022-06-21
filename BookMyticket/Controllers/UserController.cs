using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MovieLibrary;

namespace BookMyticket.Controllers
{
    public class UserController : Controller
    {
        int count = 1;
        bool flag = true;


        CinemaContext dc = new CinemaContext();
        Class1 ob = new Class1();
        public ViewResult home()
        {
            ViewData["username"] = HttpContext.Session.GetString("uid");
            var s = dc.Movies.ToList();
            return View(s);
        }
        [HttpGet]
        public ActionResult registration()
        {
            return View();

        }

        [HttpPost]
        public ActionResult registration(Usertable r)
        {

            if (ModelState.IsValid)
            {
                dc.Usertables.Add(r);
                int i = dc.SaveChanges();


                if (i > 0)
                {

                    ViewData["a"] = "New User created successfully!!!";
                }
                else
                {
                    ViewData["a"] = "Error occured try after some time";
                }

                return View();
            }
            else
            {
                return View();
            }

        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(IFormCollection frm)
        {
            
            string uid = frm["uid"];
            string pwd = frm["pwd"];
            HttpContext.Session.SetString("uid", uid);
            var res = (from c in dc.Usertables
                       where c.UserName == uid && c.Userpassword == pwd
                       select c).Count();





            if (res > 0)
            {
                return RedirectToAction("home");

            }
            else
            {
                ViewData["a"] = "In-Valid user";
            }

            return View();

        }
        public ViewResult moviedetails(string myitemname)
        {
            var result = dc.Movies.ToList().Where(c => c.MovieName == myitemname);

            //var res = from t in dc.Movies
            //          where t.MovieName == myitemname
            //          select t;



            return View(result);
        }
        public ActionResult Search(string tofind)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (tofind != "" && tofind != null)
                    {
                        List<Movie> res = ob.displaySearch(tofind);
                        return View(res);
                    }

                }
                catch (Exception error)
                {
                    Console.WriteLine(error.StackTrace);
                }
            }
            return View();


        }
        [HttpGet]
        public ViewResult Contact()
        {// logic for contact page goes here

            return View();

        }
        [HttpPost]

        public ActionResult Contact(Contactu c)
        {// logic for contact page goes here
            if (ModelState.IsValid)
            {

                int i = ob.contact(c);

                if (i > 0)
                {
                    ViewData["f"] = "  send successfully";
                }
                else
                {
                    ViewData["f"] = "un successfull...";
                }

                return View();
            }
            else
            {
                return View();
            }
        }
        public ActionResult Aboutus()
        {
            return View();
        }
         [HttpGet]
        public ActionResult Booknow()
        {


            return View();

        }
        [HttpPost]
        public ActionResult Booknow(int Number, string seats, Booking a, int b,string myitemname,int myitemid)
        {
            if (HttpContext.Session.GetString("uid") == null)
            {
                return RedirectToAction("Login");
            }

            else
            {
                string seatno = a.SeatNo.ToString();
                string[] seatnameArray = seatno.Split(',');
                //count = seatnameArray.Length;
                ViewData["Moviename"] = myitemname;
                ViewData["Movieid"]=myitemid;
                ViewData["Username"] = HttpContext.Session.GetString("uid");
                ViewData["Totalprice"] = 120 * b;

                if (checkseat(seatno) == false)
                {
                    foreach (var item in seatnameArray)
                    {

                        //Booking ab=new Booking();
                        //ab.Seatsname = seats;
                        dc.Add(new Booking { SeatNo =seatno, Totalprice = 120 * b, NoOfSeats = b,MovieName=myitemname,MovieId=myitemid,UserName= HttpContext.Session.GetString("uid") });
                    }
                    dc.Bookings.Add(a);
                    int i = dc.SaveChanges();
                    if (i > 0)
                    {
                        ViewData["a"] = "Booking confirmed";
                        var email = new MimeMessage();
                        email.Sender = MailboxAddress.Parse("movieticketfromgroup4@gmail.com");
                        email.To.Add(MailboxAddress.Parse("movieticketfromgroup4@gmail.com"));
                        email.Subject = "Booking confirmed";
                        email.Body = new TextPart(TextFormat.Html) { Text = $"<h1>Hi kavi ur booking is confirmed for Radhe shyam </h1>" };

                        // send email
                        using var smtp = new SmtpClient();
                        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                        smtp.Authenticate("movieticketfromgroup4", "Group4@123");
                        smtp.Send(email);
                        smtp.Disconnect(true);
                        ViewData["b"] = "Mailsended succesfully";
                    }
                    else
                    {
                        ViewData["a"] = "Booking not confirmed";

                    }
                }
            }

            //Booking ab=new Booking();
            //ab.Seatsname = seats;
            //ab.NoofTickets = Number;
            //ab.Totalprice = 120;
            //dc.Bookings.Add(ab);
            //int i=dc.SaveChanges();
           

            return View();
        }
        [HttpGet]
        private bool checkseat(string seatno)
        {
            //throw new NotImplementedException();
            string seats = seatno;
            string[] seatreserved = seats.Split(',');
            var seatnoList = from t in dc.Bookings
                             select t;
            foreach (var item in seatnoList)
            {
                string alreadybooked = item.SeatNo;
                foreach (var item1 in seatreserved)
                {
                    if (item1 == alreadybooked)
                    {
                        flag = false;
                        break;
                    }
                }
            }
            if (flag == false)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        [HttpPost]
        public ActionResult checkseat(DateTime moviedate, Booking booknow)
        {
            string seatno = string.Empty;
            var movielist = dc.Bookings.Where(a => a.MovieDate == moviedate).ToList();
            if (movielist != null)
            {
                var getseatno = movielist.Where(b => b.SeatNo == seatno).ToList();
                if (getseatno != null)
                {
                    seatno = seatno + " " + seatno;
                }
                TempData["sno"] = "already booked" + seatno;
            }
            return View();
        }
        //[HttpGet]
        //public ActionResult mailform()
        //{
        //    return View();

        //}
        //[HttpPost]
        //public ActionResult mailform(string toaddress, string uname, string pwd)
        //{

        //    var email = new MimeMessage();
        //    email.Sender = MailboxAddress.Parse("movieticketfromgroup4@gmail.com");
        //    email.To.Add(MailboxAddress.Parse("movieticketfromgroup4@gmail.com"));
        //    email.Subject = "Booking confirmed";
        //    email.Body = new TextPart(TextFormat.Html) { Text = $"<h1>Hi kavi ur booking is confirmed for Radhe shyam </h1>" };

        //    // send email
        //    using var smtp = new SmtpClient();
        //    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        //    smtp.Authenticate("movieticketfromgroup4", "Group4@123");
        //    smtp.Send(email);
        //    smtp.Disconnect(true);
        //    ViewData["a"] = "Mailsended succesfully";
        //    return View();




        //}
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("uid");
            return View();
        }


    }
}
