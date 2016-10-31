using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Text;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CountdownSolver.Controllers
{
    [Route("api/[controller]")]
    public class ReverseStringController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{input}")]
        public JsonResult Get(string input)
        {
            StringBuilder output = new StringBuilder();
            char[] inputArray = input.ToCharArray();
            for(int index=inputArray.Length-1; index > -1; index--)
            {
                output.Append(inputArray[index]);
            }

            IDictionary outputDictionary = new Dictionary<string, string>();
            outputDictionary.Add("value",output.ToString());
            return Json(outputDictionary);
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
