using System.Collections.Generic;
using System.Linq;
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

        public object GetOwnedKits(string uid)
        {
            return _client.Cypher
                .OptionalMatch("(user:User)-[OWNS]-(kit:Kit)")
                .Where((User user) => user.Uid == uid)
                .Return((user, kit) => new {
                    User = user.As<User>(),
                    Kits = kit.CollectAs<Kit>()
                })
                .Results.First();
        }

        public Kit GetKitDetails(string uid)
        {
            return _client.Cypher
                .Match("(kit:Kit)")
                .Where((Kit kit) => kit.Uid == uid)
                .Return(kit => kit.As<Kit>())
                .Results.First();
        }

        public Kit ClaimKit(string uid, string token)
        {
            var newUser = new User { Uid = uid };
            // Create user in database if doesn't already exist
            _client.Cypher
                .Merge("(user:User { Uid: {newUid} })")
                .OnCreate()
                .Set("user = {newUser}")
                .WithParams(new {
                    newUid = uid,
                    newUser
                })
                .ExecuteWithoutResults();

            // Check if node has already been claimed
            IEnumerable<Kit> ExistingRel = _client.Cypher
                .OptionalMatch("(u:User)-[:OWNS]-(k:Kit)")
                .Where((Kit k) => k.Token == token)
                .Return(k => k.As<Kit>())
                .Results;
            if (ExistingRel.First() != null)
            {
                // Kit has already been claimed
                return null;
            }

            // Relate kit node to user node
            return _client.Cypher
                .Match("(claimer:User)", "(target:Kit)")
                .Where((User claimer) => claimer.Uid == uid)
                .AndWhere((Kit target) => target.Token == token)
                .CreateUnique("(claimer)-[:OWNS]->(target)")
                .Return(target => target.As<Kit>())
                .Results.First();
        }

        public Kit CreateKit(Kit k)
        {
            return _client.Cypher
                .Merge("(kit:Kit { Uid: {newUid} })")
                .OnCreate()
                .Set("kit = {k}")
                .WithParams(new
                {
                    newUid = k.Uid,
                    k
                })
                .Return(kit => kit.As<Kit>())
                .Results.First();
        }
    }
}