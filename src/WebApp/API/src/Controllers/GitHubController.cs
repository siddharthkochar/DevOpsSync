using DevOpsSync.WebApp.Services;
using DevOpsSync.WebApp.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevOpsSync.WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private const string Organization = "00004";
        private const string Project = "devops-sync";
        private readonly IDataStore dataStore;
        private readonly IGitHubService _gitHubService;
        private readonly ISlackService _slackService;
        private readonly IDevOpsService _devOpsService;

        public GitHubController(
            IDataStore dataStore,
            IGitHubService gitHubService,
            ISlackService slackService,
            IDevOpsService devOpsService)
        {
            this.dataStore = dataStore;
            _gitHubService = gitHubService;
            _slackService = slackService;
            _devOpsService = devOpsService;
        }

        [HttpGet("initialize")]
        public IActionResult Initialize()
        {
            var state = Guid.NewGuid().ToString();
            dataStore.Storage["github-state"] = state;
            return Redirect(_gitHubService.GetConsentUrl(state));
        }

        [HttpGet("auth")]
        public async Task<IActionResult> Auth([FromQuery] string code, [FromQuery] string state)
        {
            var githubState = (string)dataStore.Storage["github-state"];
            if (githubState != state)
            {
                BadRequest();
            }

            var accessToken = await _gitHubService.GetAccessTokenAsync(code);
            Response.Cookies.Append("github-token", accessToken);
            return Redirect("http://localhost:3000/services/action/github");
        }

        [HttpGet("webhook/create")]
        public async Task CreateWebHookAsync()
        {
            var events = new List<string> { "push", "pull_request" };
            string token = Request.Cookies["github-token"];
            await _gitHubService.CreateWebHookAsync(token, events, "sidkcr", "DevOpsSync");
        }

        [HttpPost("webhook/handle")]
        public async Task Handle([FromBody] object content)
        {
            var xGithubEvent = Request.Headers["X-GitHub-Event"].ToString();
            (string state, List<string> workItemIds) = _gitHubService.GetEventInformation(xGithubEvent, content.ToString());

            foreach (var workItemId in workItemIds)
            {
                await SetItemStatus(Organization, Project, Convert.ToInt32(workItemId), state);
            }

            if (xGithubEvent == "pull_request")
            {
                PostMessage("general", $"New pull request - https://github.com/sidkcr/DevOpsSync/pulls");
            }
        }

        private async Task SetItemStatus(
            string organization, string project, int workItemId, string status)
        {
            await _devOpsService.SetWorkItemStatus(organization, project, workItemId, status);
        }

        private void PostMessage(string channel, string text)
        {
            var accessToken = Request.Cookies["slack-token"];
            _slackService.PostMessage(accessToken, channel, text);
        }
    }
}