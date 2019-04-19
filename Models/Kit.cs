using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace bdapi_kits.Models
{
    public class Kit
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }
        
        [JsonProperty("token")]
        public string Token { get; set; }
        
        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
