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
    //[Authorize] -- deprecated for bdapi-auth until we have JWTs
    [Route("kit")]
    // [Produces("application/json")]
    [ApiController]
    public class KitController : ControllerBase
    {
        private readonly KitService _kitService;
        private readonly AuthService _authService;
        
        public KitController(KitService kitService, AuthService authService)
        {
            _kitService = kitService;
            _authService = authService;
        }
        
        /* User actions */
        
        // Get your current claimed kits
        // GET /kit/me
        [HttpGet("me")]
        public object GetMyKits()
        {
            string Uid = _authService.GetUserUidFromToken(Request.Headers["Authorization"]).Result;
            if (Uid == null)
            {
                throw new ArgumentException();
            }
            object MyKits = _kitService.GetOwnedKits(Uid);

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
            string Uid = _authService.GetUserUidFromToken(Request.Headers["Authorization"]).Result;
            if (Uid == null)
            {
                throw new ArgumentException();
            }
            Kit ClaimedKit = _kitService.ClaimKit(Uid, token);
            return ClaimedKit;
        }

        /* Administrative actions */

        // Add an available kit that's ready to be claimed
        // POST /kit
        [HttpPost]
        public Kit Post([FromBody] Kit kit)
        {
            string Uid = _authService.GetUserUidFromToken(Request.Headers["Authorization"]).Result;
            if (Uid == null)
            {
                throw new ArgumentException();
            }
            // TODO: IsAdmin? Method
            //if (Uid == "bucksuid")
            //{
                // Generate a random UID for the kit
                kit.Uid = Guid.NewGuid().ToString();
                // Add the kit
                Kit NewKit = _kitService.CreateKit(kit);
                return NewKit;
            //}
            // Not sent from an administrative account
            return null;
        }
    }
}
