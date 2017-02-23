using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            var timeSpan = JsonConvert.DeserializeObject<TimeSpan>(value);
            _scores.Add(timeSpan);
        }

    }
}
