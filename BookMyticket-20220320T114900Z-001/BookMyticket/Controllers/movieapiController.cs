using Microsoft.AspNetCore.Mvc;
using MovieLibrary;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookMyticket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class movieapiController : ControllerBase
    {
        CinemaContext dc=new CinemaContext();

        // GET: api/<movieapiController>
        [HttpGet]
        public IEnumerable<Feedback> Get()
        {
            var res = dc.Feedbacks;
            return res;
        }

        // GET api/<movieapiController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<movieapiController>
        [HttpPost]
        public Feedback Post(Feedback s)
        {
            dc.Feedbacks.Add(s);
            int i = dc.SaveChanges();
            if (i > 0)
            {
                return s;
            }
            else
            {
                return null;
            }
        }

        // PUT api/<movieapiController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<movieapiController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
