using DevOpsSync.WebApp.API.Models.GitHub;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace DevOpsSync.WebApp.API.Code
{
    public interface IGitHub
    {
        Task<Token.Response> GetTokenAsync(Token.Request parameters);
    }

    public class GitHub : IGitHub
    {
        public async Task<Token.Response> GetTokenAsync(Token.Request parameters)
        {
            var client = new RestClient("https://github.com/login");
            client.Timeout = -1;
            var request = new RestRequest("/oauth/access_token", Method.POST);
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_id", parameters.ClientId);
            request.AddParameter("client_secret", parameters.CilentSecret);
            request.AddParameter("code", parameters.Code);
            IRestResponse response = await client.ExecuteAsync(request);
            var content = JsonConvert.DeserializeObject<Token.Response>(response.Content);
            return content;
        }
    }
}
