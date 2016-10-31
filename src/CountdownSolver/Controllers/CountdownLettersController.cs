using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CountdownSolver.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CountdownSolver.Controllers
{
    [Route("countdownsolver/[controller]")]
    public class CountdownLettersController : Controller
    {
        // GET: api/values
        [HttpGet]
        public JsonResult Get()
        {
            ICollection<string> wordsFound = new List<string>();
            return Json(wordsFound);
        }

        // GET api/values/5
        [HttpGet("{inputCharacters}")]
        public JsonResult Get(string inputCharacters)
        {
            ICollection<string> wordsFound = WordList.findAllWords(inputCharacters);
            return Json(wordsFound);
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
