using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Neo4jClient;
using Microsoft.Extensions.Configuration;
using bdapi_kits.Models;

namespace bdapi_kits.Services
{
  public class KitService
  {
    private readonly GraphClient _client;
    public KitService(IConfiguration config) {
      var graphClient = new GraphClient(new System.Uri(config.GetConnectionString("KitDb")), "neo4j", "neoneo");
      graphClient.Connect();
      _client = graphClient;
    }
    
    public IEnumerable<Kit> GetOwnedKits(string Uid)
    {
      return _client.Cypher
        .OptionalMatch("(user:User)-[OWNS]-(kit:Kit)")
        .Where((User user) => user.Uid == Uid)
        .Return(Kit => Kit.As<Kit>())
        .Results;
    }

    public IEnumerable<Kit> GetKitDetails(string Kid)
    {
      return _client.Cypher
        .Match("(kit:Kit)")
        .Where((Kit kit) => kit.Uid == Kid)
        .Return(kit => kit.As<Kit>())
        .Results;
    }

    public IEnumerable<Kit> ClaimKit(string Uid, string Token)
    {
      // Create user in database if doesn't already exist
      _client.Cypher
        .Merge("(user:User { Uid: {uid} })")
        .OnCreate()
        // .Set("user = {newUser}")
        .WithParams(new {
          uid = Uid,
          // newUser
        })
        .ExecuteWithoutResults();

      // Relate kit node to user node
      return _client.Cypher
        .Match("(claimer:User)", "(target:Kit")
        .Where((User claimer) => claimer.Uid == Uid)
        .AndWhere((Kit target) => target.Token == Token)
        .CreateUnique("claimer-[:OWNS]->target")
        .Return(target => target.As<Kit>())
        .Results;
    }
  }
}