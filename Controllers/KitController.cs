using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using bdapi_kits.Models;
using bdapi_kits.Services;
using Newtonsoft.Json;

namespace bdapi_kits.Controllers
{
    [Authorize]
    [Route("kit")]
    // [Produces("application/json")]
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
        // GET /kit/me
        [HttpGet("me")]
        public IEnumerable<Kit> GetMyKits()
        {
            // Console.WriteLine(HttpContext.User);
            IEnumerable<Kit> kits = _kitService.GetOwnedKits("456");
            return kits;
        }
        
        // Get the details for some claimed kit
        // GET /kit/id/{kid}
        [HttpGet("id/{uid}")]
        public Kit GetKitDetails(string uid)
        {
            Kit kit = _kitService.GetKitDetails(uid).First();
            return kit;
        }
        
        // Claim a kit
        // PUT /kit/51dbd231dfs241dsdae23c4a
        [HttpPut("{token}")]
        public void Put(string token, [FromBody] string value)
        {
            Kit kit = _kitService.ClaimKit("456", token).First();
        }
        
        /* Administrative actions */
        
        // Add an available kit that's ready to be claimed
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
