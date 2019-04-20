using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using bdapi_kits.Models;
using bdapi_kits.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        public IEnumerable GetMyKits()
        {
            string Email = (string) JObject.Parse(HttpContext.User.Claims.
                ToList().Last().Value)["identities"]["email"][0];
            // TODO: Get first from IEnumerable rather than an array            IEnumerable kits = _kitService.GetOwnedKits(Email);
            return kits;
        }
        
        // Get the details for some claimed kit
        // GET /kit/id/{uid}
        [HttpGet("id/{uid}")]
        public IEnumerable GetKitDetails(string uid)
        {
            // TODO: Get first from IEnumerable rather than an array
            IEnumerable kit = _kitService.GetKitDetails(uid);
            return kit;
        }
        
        // Claim a kit
        // PUT /kit/51dbd231dfs241dsdae23c4a
        [HttpPut("{token}")]
        public IEnumerable Put(string token)
        {
            string Email = (string)JObject.Parse(HttpContext.User.Claims.
                ToList().Last().Value)["identities"]["email"][0];
            // TODO: Get first from IEnumerable rather than an array
            IEnumerable kit = _kitService.ClaimKit(Email, token);
            return kit;
        }

        /* Administrative actions */

        // Add an available kit that's ready to be claimed
        // POST /kit
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
    }
}
