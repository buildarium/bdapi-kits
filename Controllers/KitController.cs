using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bdapi_kits.Models;
using bdapi_kits.Services;

namespace bdapi_kits.Controllers
{
    [Route("kit")]
    [ApiController]
    public class KitController : ControllerBase
    {
        private readonly KitService _kitService;

        public KitController(KitService kitService)
        {
            _kitService = kitService;
        }

        /* User actions */

        // Get your current claimed kits
        // GET /kit
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            IEnumerable<Kit> kits = _kitService.GetOwnedKits();
            Console.WriteLine(kits.ToString());
            return new string[] { "value1", "value2" };
        }

        // Claim a kit
        // PUT /kit/51dbd231dfs241dsdae23c4a
        [HttpPut("{token}")]
        public void Put(string token, [FromBody] string value)
        {
        }

        /* Administrative actions */

        // Add an available kit for sale
        // POST /kit
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // Remove a kit
        // DELETE kit/51dbd231dfs241dsdae23c4a
        [HttpDelete("{token}")]
        public void Delete(string token)
        {
        }
    }
}
