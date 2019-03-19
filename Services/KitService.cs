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
    
    public IEnumerable<Kit> GetOwnedKits()
    {
      return _client.Cypher
        .Match("(kit:Kit)")
        .Return(kit => kit.As<Kit>())
        .Results;
    }
  }
}