using DevOpsSync.WebApp.Services.Models.Slack;
using DevOpsSync.WebApp.Utility;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace DevOpsSync.WebApp.Services
{
    public interface ISlackService
    {
        string GetConsentUrl(string state);
        Task<string> GetAccessTokenAsync(string code);
        Task PostMessage(string accessToken, string channel, string text);
    }

    public class SlackService : ISlackService
    {
        private readonly ClientSettings _settings;
        private readonly IRestClient _client;

        public SlackService(IOptions<Settings> settings, IRestClient client)
        {
            _settings = settings.Value.Slack;
            _client = client;
        }

        public string GetConsentUrl(string state) 
        {
            var authorizeUrl = $"https://slack.com/oauth/v2/authorize?" +
                "scope=chat:write,channels:read,im:read,users:read,users:read.email&" +
                $"client_id={_settings.ClientId}&" +
                $"redirect_uri={_settings.RedirectUrl}&" +
                $"state={state}";

            return authorizeUrl;
        }

        public async Task<string> GetAccessTokenAsync(string code)
        {
            var baseUrl = "https://slack.com/api/oauth.v2.access" +
                            "?scope=chat:write,channels:read" +
                           $"&client_id={_settings.ClientId}" +
                           $"&code={code}" +
                           $"&client_secret={_settings.ClientSecret}";

            _client.BaseUrl = new Uri(baseUrl);
            var request = new RestRequest(Method.POST);
            var res = await _client.ExecuteAsync<Token.Response>(request, new System.Threading.CancellationToken());
            return res.Data.AccessToken;
        }

        public async Task PostMessage(string accessToken, string channel, string text)
        {
            _client.BaseUrl = new Uri("https://slack.com/api/chat.postMessage");
            var request = new RestRequest();
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json",
                $@"
                {{
                    ""channel"": ""{channel}"",
                    ""text"": ""{text}""
                }}", ParameterType.RequestBody);

            await _client.ExecuteAsync(request, Method.POST);
        }
    }
}
