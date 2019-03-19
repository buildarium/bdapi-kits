using System.Collections.Generic;
using System.Linq;
using Neo4j.Driver.V1;

namespace bdapi_kits.Models
{
  public class Kit
  {
    public string Token { get; }
    public UserModel Owner { get; }
  }
}
