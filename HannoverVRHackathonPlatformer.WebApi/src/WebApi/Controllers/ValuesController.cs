using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private static List<TimeSpan> _scores = new List<TimeSpan>();

        // GET api/values
        [HttpGet]
        public JsonResult Get()
        {
            return Json(_scores);
        }


        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            var timeSpan = TimeSpan.FromMilliseconds(double.Parse(value));
            _scores.Add(timeSpan);
        }

    }
}
