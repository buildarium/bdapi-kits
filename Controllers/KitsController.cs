using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace bdapi_kits.Controllers
{
    [Route("kit")]
    [ApiController]
    public class KitsController : ControllerBase
    {
        /* User actions */

        // Get your current claimed kits
        // GET /kit
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
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
