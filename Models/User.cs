using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace bdapi_kits.Models
{
    public class User
    {
        [JsonProperty("uid")]
        public string Uid { get; set; }
    }
}
