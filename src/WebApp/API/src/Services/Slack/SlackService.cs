using Newtonsoft.Json;
using RestSharp;
using DevOpsSync.WebApp.API.Models.Slack;

namespace DevOpsSync.WebApp.API.Services.Slack
{
    public class SlackService
    {
        private string ClientId { get; }
        private string ClientSecret { get; }
        private string AccessToken { get; set; }

        public SlackService(
            string clientId, string clientSecret, string authCode)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            InitializeServiceWithAuthCode(authCode);
        }

        private void InitializeServiceWithAuthCode(string authCode)
        {
            var client = new RestClient(
                "https://slack.com/api/oauth.v2.access?scope=chat:write,channels:read" +
                $"&client_id={ClientId}&code={authCode}&client_secret={ClientSecret}");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var content = JsonConvert.DeserializeObject<Token.Response>(response.Content);
            AccessToken = content.AccessToken;
        }

        public void PostMessage(string channel, string text)
        {
            var client = new RestClient("https://slack.com/api/chat.postMessage");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Authorization", $"Bearer {AccessToken}");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", 
                $@"
                {{
                    ""channel"": ""{channel}"",
                    ""text"": ""{text}""
                }}",  ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }
    }
}