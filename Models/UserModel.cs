using System;
using Neo4j.Driver.V1;

namespace bdapi_kits.Models
{
  public class UserModel
  {
    public const string UsernameKey = "username";

    public UserModel(INode node)
    {
      if (node == null)
      {
        throw new ArgumentNullException(nameof(node));
      }
    }

    public string Username { get; }
  }
}