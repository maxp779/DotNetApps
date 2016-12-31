using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CountdownSolver.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CountdownSolver.Controllers
{
    [Route("countdownsolver/[controller]")]
    public class CountdownNumbersController : Controller
    {
        [HttpGet("{jsonString}")]
        public JsonResult Get(string jsonString)
        {
            NumbersInput numbersGameInput = JsonConvert.DeserializeObject<NumbersInput>(jsonString);
            ICollection<string> output;
            if(numbersGameInput.speed == "fast")
            {
                CountdownNumbersCalculatorFast calculator = new CountdownNumbersCalculatorFast();
                List<int> intList = numbersGameInput.numbers.Select(s => int.Parse(s)).ToList();

                output = calculator.calculate(intList, Convert.ToInt32(numbersGameInput.target));
            }
            else
            {
                CountdownNumbersCalculatorSlow calculator = new CountdownNumbersCalculatorSlow(numbersGameInput.numbers, numbersGameInput.target);
                output = calculator.calculate();
            }
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

        public class NumbersInput
        {
            public List<string> numbers { get; set; }
            public string target { get; set; }
            public string speed { get; set; }

        }
    }
}
