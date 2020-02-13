using DevOpsSync.WebApp.Services.Models.GitHub.Events;
using DevOpsSync.WebApp.Utility;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DevOpsSync.WebApp.Services
{
    public interface IGitHubService
    {
        string GetConsentUrl(string state);
        Task<string> GetAccessTokenAsync(string code);
        Task CreateWebHookAsync(string token, IEnumerable<string> events, string repoOwner, string repoName);
        (string, List<string>) GetEventInformation(string gitHubEvent, object content);
    }

    public class GitHubService : IGitHubService
    {
        private readonly Settings _settings;
        private readonly GitHubClient _client;

        public GitHubService(IOptions<Settings> settings)
        {
            _settings = settings.Value;
            _client = new GitHubClient(new ProductHeaderValue("DevOpsSync"));
        }

        public string GetConsentUrl(string state)
        {
            var request = new OauthLoginRequest(_settings.GitHub.ClientId)
            {
                RedirectUri = new Uri(_settings.GitHub.RedirectUrl),
                State = state,
                Scopes = { "admin:repo_hook" }
            };

            var oauthLoginUrl = _client.Oauth.GetGitHubLoginUrl(request);
            return oauthLoginUrl.AbsoluteUri;
        }

        public async Task<string> GetAccessTokenAsync(string code)
        {
            var authRequest = new OauthTokenRequest(_settings.GitHub.ClientId, _settings.GitHub.ClientSecret, code);
            OauthToken token = await _client.Oauth.CreateAccessToken(authRequest);
            return token.AccessToken;
        }

        public async Task CreateWebHookAsync(string token, IEnumerable<string> events, string repoOwner, string repoName)
        {
            _client.Credentials = new Credentials(token);

            var hookConfig = new Dictionary<string, string>
            {
                { "url", $"{_settings.AppRootUrl}/api/github/webhook/handle" },
                { "content_type", "application/json" }
            };

            var hook = new NewRepositoryHook("web", hookConfig)
            {
                Active = true,
                Events = events
            };

            await _client.Repository.Hooks.Create(repoOwner, repoName, hook);
        }

        public (string, List<string>) GetEventInformation(string gitHubEvent, object content)
        {
            var message = string.Empty;
            var state = string.Empty;
            List<string> workItems = new List<string>();
            switch (gitHubEvent)
            {
                case "push":
                    var pushEvent = JsonConvert.DeserializeObject<PushEvent>(content.ToString());
                    message = pushEvent.commits.First().message;
                    state = "In Progress";
                    break;
                case "pull_request":
                    var pullRequestEvent = JsonConvert.DeserializeObject<PullRequestEvent>(content.ToString());
                    message = pullRequestEvent.pull_request.body;
                    state = pullRequestEvent.action == "opened"
                        ? "Approved"
                        : "Done";
                    break;
            }

            var matches = Regex.Matches(message, @"#\d+");
            foreach (Match match in matches)
            {
                workItems.Add(match.Value.Replace("#", string.Empty));
            }

            return (state, workItems);
        }
    }
}
