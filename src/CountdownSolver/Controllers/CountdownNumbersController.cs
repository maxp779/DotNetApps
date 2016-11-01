using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CountdownSolver.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CountdownSolver.Controllers
{
    [Route("countdownsolver/[controller]")]
    public class CountdownNumbersController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{numbersArray}")]
        public JsonResult Get(string numbersArray)
        {
            List<int> numbers = JsonConvert.DeserializeObject<List<int>>(numbersArray);
            int targetNumber = numbers.Last<int>(); //target number is the final number in the array
            numbers.RemoveAt(numbers.Count - 1);
            CountdownNumbersCalculator calculator = new CountdownNumbersCalculator();
            HashSet<string> output = calculator.calculate(numbers, targetNumber);
            return Json(output);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
