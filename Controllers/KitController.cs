using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using bdapi_kits.Models;
using bdapi_kits.Services;
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
        public object GetMyKits()
        {
            string Email = (string) JObject.Parse(HttpContext.User.Claims.
                ToList().Last().Value)["identities"]["email"][0];
            object MyKits = _kitService.GetOwnedKits(Email);
            return MyKits;
        }

        // Get some user's claimed kits
        // GET /kit/user/{uid}
        [HttpGet("user/{uid}")]
        public object GetOtherKits(string uid)
        {
            object OtherKits = _kitService.GetOwnedKits(uid);
            return OtherKits;
        }
        
        // Get the details for some claimed kit
        // GET /kit/id/{uid}
        [HttpGet("id/{uid}")]
        public Kit GetKitDetails(string uid)
        {
            Kit SomeKit = _kitService.GetKitDetails(uid);
            return SomeKit;
        }

        // Claim a kit
        // PUT /kit/{token}
        [HttpPut("{token}")]
        public Kit Put(string token)
        {
            string Email = (string)JObject.Parse(HttpContext.User.Claims.
                ToList().Last().Value)["identities"]["email"][0];
            Kit ClaimedKit = _kitService.ClaimKit(Email, token);
            return ClaimedKit;
        }

        /* Administrative actions */

        // Add an available kit that's ready to be claimed
        // POST /kit
        [HttpPost]
        public Kit Post([FromBody] Kit k)
        {
            string Email = (string)JObject.Parse(HttpContext.User.Claims.
                ToList().Last().Value)["identities"]["email"][0];
            if (Email == "buck@bucktower.net")
            {
                Kit NewKit = _kitService.CreateKit(k);
                return NewKit;
            }
            // Not sent from an administrative account
            return null;
        }
    }
}
