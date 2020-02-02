using DevOpsSync.WebApp.API.Models.GitHub.Events;
using DevOpsSync.WebApp.API.Services.VSTS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Octokit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DevOpsSync.WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly Settings config;
        private readonly IDataStore dataStore;
        private readonly GitHubClient client;

        public GitHubController(
            IOptions<Settings> config,
            IDataStore dataStore)
        {
            this.config = config.Value;
            this.dataStore = dataStore;
            client = new GitHubClient(new ProductHeaderValue("DevOpsSync"));
        }

        [HttpGet]
        public string GetConsentUrl()
        {
            var state = Guid.NewGuid().ToString();
            dataStore.Storage.Add("github-state", state);

            var request = new OauthLoginRequest(config.GitHub.ClientId)
            {
                RedirectUri = new Uri(config.GitHub.RedirectUrl),
                State = state,
                Scopes = { "admin:repo_hook" }
            };

            var oauthLoginUrl = client.Oauth.GetGitHubLoginUrl(request);
            return oauthLoginUrl.AbsoluteUri;
        }

        [HttpGet("auth")]
        public async Task Auth([FromQuery] string code, [FromQuery] string state)
        {
            var githubState = dataStore.Storage["github-state"];
            if (githubState != state)
            {
                Unauthorized();
            }

            var authRequest = new OauthTokenRequest(config.GitHub.ClientId, config.GitHub.ClientSecret, code);
            var token = await client.Oauth.CreateAccessToken(authRequest);
            Response.Cookies.Append("github-token", token.AccessToken);
        }

        [HttpGet("webhook/create")]
        public async Task CreateWebHook()
        {
            client.Credentials = new Credentials(dataStore.Storage["GitHubToken"]);

            var hookConfig = new Dictionary<string, string>
            {
                { "url", $"{config.AppRootUrl}/api/github/webhook/handle" },
                { "content_type", "application/json" }
            };

            var hook = new NewRepositoryHook("web", hookConfig)
            {
                Active = true,
                Events = new List<string> { "push", "pull_request" }
            };

            await client.Repository.Hooks.Create("sidkcr", "DevOpsSync", hook);
        }

        [HttpPost("webhook/handle")]
        public void Handle([FromBody] object content)
        {
            var xGithubEvent = Request.Headers["X-GitHub-Event"].ToString();
            var message = string.Empty;
            var state = string.Empty;
            switch (xGithubEvent)
            {
                case "push":
                    var pushEvent = JsonConvert.DeserializeObject<PushEvent>(content.ToString());
                    message = pushEvent.commits.First().message;
                    state = "In-progress";
                    break;
                case "pull_request":
                    var pullRequestEvent = JsonConvert.DeserializeObject<PullRequestEvent>(content.ToString());
                    message = pullRequestEvent.pull_request.body;
                    state = pullRequestEvent.action == "opened"
                        ? "In-verify"
                        : "Done";
                    break;
            }

            string workItemId = Regex.Match(message, @"#\d+").Value;
        }

        private void SetItemStatus(
            string organization, string project, int workItemId, string status)
        {
            var service = (VSTSService)dataStore.ObjectStorage[Constants.VSTSServiceKey];
            service.SetWorkItemStatus(organization, project, workItemId, status);
        }
    }
}