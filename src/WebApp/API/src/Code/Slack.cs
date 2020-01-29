using DevOpsSync.WebApp.API.Models.Slack;
using Newtonsoft.Json;
using RestSharp;
using System.Threading.Tasks;

namespace DevOpsSync.WebApp.API.Code
{
    public interface ISlack
    {
        Task<Token.Response> GetTokenAsync(Token.Request parameters);
    }

    public class Slack : ISlack
    {
        public async Task<Token.Response> GetTokenAsync(Token.Request parameters)
        {
            var client = new RestClient("https://slack.com/api");
            client.Timeout = -1;
            var request = new RestRequest("/oauth.v2.access", Method.GET);
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
