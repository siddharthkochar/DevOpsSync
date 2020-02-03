using DevOpsSync.WebApp.API.Models.GitHub.Events;
using DevOpsSync.WebApp.API.Services.Slack;
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
        private const string Organization = "00004";
        private const string Project = "devops-sync";
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

        [HttpGet("initialize")]
        public IActionResult Initialize()
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
            return Redirect(oauthLoginUrl.AbsoluteUri);
        }

        [HttpGet("auth")]
        public async Task Auth([FromQuery] string code, [FromQuery] string state)
        {
            var githubState = (string)dataStore.Storage["github-state"];
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
            client.Credentials = new Credentials((string)dataStore.Storage["GitHubToken"]);

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
                        ? "Approved"
                        : "Done";
                    break;
            }

            var matches = Regex.Matches(message, @"#\d+");
            foreach (Match match in matches)
            {
                string workItemId = match.Value.Replace("#", string.Empty);
                SetItemStatus(Organization, Project, Convert.ToInt32(workItemId), state);
            }

            if (xGithubEvent == "pull_request")
            {
                PostMessage("general", $"New pull request - https://dev.azure.com/{Organization}/_git/{Project}/pullrequests");
            }
        }

        private void SetItemStatus(
            string organization, string project, int workItemId, string status)
        {
            var service = (VSTSService)dataStore.Storage[Services.VSTS.Constants.VSTSServiceKey];
            service.SetWorkItemStatus(organization, project, workItemId, status);
        }


        private void PostMessage(string channel, string text)
        {
            var service = (SlackService)dataStore.Storage[Services.Slack.Constants.SlackKey];
            service.PostMessage(channel, text);
        }
    }
}