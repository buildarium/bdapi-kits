using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;


namespace bdapi_kits.Models
{
    public class Kit
    {
        [Key]
        public string Uid { get; set; }
        
        public string Token { get; set; }
        
        public string Type { get; set; }
    }
}
