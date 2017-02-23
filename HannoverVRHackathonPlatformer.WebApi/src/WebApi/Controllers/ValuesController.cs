using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private static List<TimeSpan> _scores = new List<TimeSpan>();

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(string.Join(",", _scores.Select(s => s.TotalMilliseconds.ToString())));
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
