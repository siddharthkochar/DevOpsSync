using Newtonsoft.Json;
using RestSharp;

namespace DevOpsSync.WebApp.API.Services.VSTS
{
    public class VSTSService
    {
        private string ClientSecret { get; }
        private string RedirectUrl { get; }
        private string RefreshToken { get; set; }
        private string AccessToken { get => RefreshAccessToken(); }

        public VSTSService(
            string clientSecret, string redirectUrl, string authCode)
        {
            ClientSecret = clientSecret;
            RedirectUrl = redirectUrl;
            InitializeServiceWithAuthCode(authCode);
        }

        public void SetWorkItemStatus(
            string organization, string project, int workItem, string status)
        {
            var client = new RestClient(
                $"https://dev.azure.com/{organization}/{project}/_apis/wit/workitems/{workItem}?api-version=5.1");
            client.Timeout = -1;
            var request = new RestRequest(Method.PATCH);
            request.AddHeader("Content-Type", "application/json-patch+json");
            request.AddHeader("Authorization", $"Bearer {AccessToken}");
            request.AddParameter("application/json-patch+json", $@"[
                {{
                    ""op"": ""replace"",
                    ""path"": ""/fields/System.State"",
                    ""value"": ""{status}""
                }}
            ]", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }

        private void InitializeServiceWithAuthCode(string authCode)
        {
            var client = new RestClient("https://app.vssps.visualstudio.com/oauth2/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer");
            request.AddParameter("client_assertion", ClientSecret);
            request.AddParameter("grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer");
            request.AddParameter("assertion", authCode);
            request.AddParameter("redirect_uri", RedirectUrl);
            IRestResponse response = client.Execute(request);

            var content = JsonConvert.DeserializeObject<Response>(response.Content);
            RefreshToken = content.RefreshToken;
        }

        public static string GetAuthUrl(string clientId, string redirectUrl)
        {
            return
            $"https://app.vssps.visualstudio.com/oauth2/authorize?client_id={clientId}&response_type=Assertion&scope=vso.work_full&redirect_uri={redirectUrl}";
        }

        private string RefreshAccessToken()
        {
            var client = new RestClient("https://app.vssps.visualstudio.com/oauth2/token");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer");
            request.AddParameter("client_assertion", ClientSecret);
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("assertion", RefreshToken);
            request.AddParameter("redirect_uri", RedirectUrl);
            IRestResponse response = client.Execute(request);

            var content = JsonConvert.DeserializeObject<Response>(response.Content);
            RefreshToken = content.RefreshToken;
            return content.AccessToken;
        }
    }
}