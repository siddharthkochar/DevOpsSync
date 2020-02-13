using DevOpsSync.WebApp.Services.Models.DevOps;
using DevOpsSync.WebApp.Utility;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace DevOpsSync.WebApp.Services
{
    public interface IDevOpsService
    {
        string GetConsentUrl(string state);
        Task SetRefreshTokenAsync(string code);
        Task SetWorkItemStatus(string organization, string project, int workItem, string status);
        Task<string> GetAccessToken();
    }

    public class DevOpsService : IDevOpsService
    {
        private readonly ClientSettings _settings;
        private readonly IRestClient _client;
        private string _refreshToken;

        public DevOpsService(IOptions<Settings> settings, IRestClient client)
        {
            _settings = settings.Value.VSTS;
            _client = client;
        }

        public async Task SetRefreshTokenAsync(string code)
        {
            _client.BaseUrl = new Uri("https://app.vssps.visualstudio.com/oauth2/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer");
            request.AddParameter("client_assertion", _settings.ClientSecret);
            request.AddParameter("grant_type", "urn:ietf:params:oauth:grant-type:jwt-bearer");
            request.AddParameter("assertion", code);
            request.AddParameter("redirect_uri", _settings.RedirectUrl);
            IRestResponse<Token.Response> response = await _client.ExecuteAsync<Token.Response>(request, new System.Threading.CancellationToken());
            _refreshToken = response.Data.RefreshToken;
        }

        public string GetConsentUrl(string state)
        {
            return $"https://app.vssps.visualstudio.com/oauth2/authorize" +
                $"?client_id={_settings.ClientId}" +
                $"&response_type=Assertion" +
                $"&scope=vso.work_full" +
                $"&redirect_uri={_settings.RedirectUrl}" +
                $"&state={state}";
        }

        public async Task SetWorkItemStatus(string organization, string project, int workItem, string status)
        {
            var token = await GetAccessToken();

            var baseUrl = $"https://dev.azure.com/{organization}/{project}/_apis/wit/workitems/{workItem}?api-version=5.1";
            _client.BaseUrl = new Uri(baseUrl);
            var request = new RestRequest(Method.PATCH);
            request.AddHeader("Content-Type", "application/json-patch+json");
            request.AddHeader("Authorization", $"Bearer {token}");
            request.AddParameter("application/json-patch+json", $@"[
                {{
                    ""op"": ""replace"",
                    ""path"": ""/fields/System.State"",
                    ""value"": ""{status}""
                }}
            ]", ParameterType.RequestBody);
            await _client.ExecuteAsync(request, Method.PATCH);
        }

        public async Task<string> GetAccessToken()
        {
            _client.BaseUrl = new Uri("https://app.vssps.visualstudio.com/oauth2/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer");
            request.AddParameter("client_assertion", _settings.ClientSecret);
            request.AddParameter("grant_type", "refresh_token");
            request.AddParameter("assertion", _refreshToken);
            request.AddParameter("redirect_uri", _settings.RedirectUrl);
            IRestResponse<Token.Response> response = await _client.ExecuteAsync<Token.Response>(request, new System.Threading.CancellationToken());
            _refreshToken = response.Data.RefreshToken;
            return response.Data.AccessToken;
        }
    }
}
