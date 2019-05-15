using System.Net.Http;
using System.Threading.Tasks;

namespace bdapi_kits.Services
{
    public class AuthService
    {
        public HttpClient Client { get; }


        public AuthService(HttpClient client)
        {
            Client = client;

            if (System.Environment.GetEnvironmentVariable("ENV") == "prod")
            {
                Client.BaseAddress = new System.Uri("bdapi-auth/");
            }
            if (System.Environment.GetEnvironmentVariable("ENV") == "test")
            {
                Client.BaseAddress = new System.Uri("http://bdapi-auth/");
            }
            else
            {
                Client.BaseAddress = new System.Uri("http://test.buildarium.com:5205/");
            }
        }

        public async Task<string> GetUserUidFromToken(string token)
        {
            Client.DefaultRequestHeaders.Add("Authorization", token);
            return await Client.GetStringAsync("auth/uid");
        }
    }
}