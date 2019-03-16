using System.Collections.Generic;
using System.Linq;
using Neo4j.Driver.V1;

namespace bdapi_kits.Models
{
  public class KitModel
  {
    public const string KitKey = "kit";
    public const string TokenKey = "token";
    public const string OwnerKey = "owner";

    public KitModel(IRecord record)
    {
      var kit = record.GetOrDefault(KitKey, (INode) null);
      if (kit != null)
      {
        TokenKey = kit.GetOrDefault<string>(TokenKey, null);
      }

      var owner = record.GetOrDefault(OwnerKey, (INode) null);
      if (owner != null)
      {
        Owner = new UserModel((INode) owner);
      }
    }

    public string Token { get; }
    public UserModel Owner { get; }
  }
}
