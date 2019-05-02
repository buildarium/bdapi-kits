using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using bdapi_kits.Models;
using bdapi_kits.Services;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System;
using System.IdentityModel.Tokens.Jwt;

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
            string Uid = HttpContext.User.Claims.Where(k => k.Type == "sub")
                .Select(v => v.Value).SingleOrDefault();
            //var jwt = new JwtSecurityToken(HttpContext.));
            //Console.WriteLine(HttpContext.User.Claims.SelectMany(x => x.Type));
            foreach (var i in HttpContext.User.Claims)
            {
                Console.WriteLine(i.Type);
                Console.WriteLine(i.Value);
            }
            Console.WriteLine(HttpContext.User.Identity.Name);
            object MyKits = _kitService.GetOwnedKits(Uid);

            return MyKits;
        }

        // Get some user's claimed kits
        // GET /kit/user/{uid}
        [AllowAnonymous]
        [HttpGet("user/{uid}")]
        public object GetOtherKits(string uid)
        {
            object OtherKits = _kitService.GetOwnedKits(uid);
            return OtherKits;
        }

        // Get the details for some claimed kit
        // GET /kit/id/{uid}
        [AllowAnonymous]
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
            string Uid = HttpContext.User.Claims.Where(k => k.Type == "sub")
                .Select(v => v.Value).SingleOrDefault();
            Kit ClaimedKit = _kitService.ClaimKit(Uid, token);
            return ClaimedKit;
        }

        /* Administrative actions */

        // Add an available kit that's ready to be claimed
        // POST /kit
        [HttpPost]
        public Kit Post([FromBody] Kit kit)
        {
            string Uid = HttpContext.User.Claims.Where(k => k.Type == "sub")
                .Select(v => v.Value).SingleOrDefault();
            if (Uid == "bucksuid")
            {
                Kit NewKit = _kitService.CreateKit(kit);
                return NewKit;
            }
            // Not sent from an administrative account
            return null;
        }
    }
}
