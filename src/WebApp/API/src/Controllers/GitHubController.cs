using DevOpsSync.WebApp.API.Services.Slack;
using DevOpsSync.WebApp.API.Services.VSTS;
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

        public GitHubController(
            IDataStore dataStore,
            IGitHubService gitHubService)
        {
            this.dataStore = dataStore;
            _gitHubService = gitHubService;
        }

        [HttpGet("initialize")]
        public IActionResult Initialize()
        {
            var state = Guid.NewGuid().ToString();
            dataStore.Storage.Add("github-state", state);
            return Redirect(_gitHubService.GetConsentUrl(state));
        }

        [HttpGet("auth")]
        public async Task Auth([FromQuery] string code, [FromQuery] string state)
        {
            var githubState = (string)dataStore.Storage["github-state"];
            if (githubState != state)
            {
                BadRequest();
            }

            var accessToken = await _gitHubService.GetAccessTokenAsync(code);
            Response.Cookies.Append("github-token", accessToken);
        }

        [HttpGet("webhook/create")]
        public async Task CreateWebHookAsync()
        {
            var events = new List<string> { "push", "pull_request" };
            string token = Request.Cookies["github-token"];
            await _gitHubService.CreateWebHookAsync(token, events, "sidkcr", "DevOpsSync");
        }

        [HttpPost("webhook/handle")]
        public void Handle([FromBody] object content)
        {
            var xGithubEvent = Request.Headers["X-GitHub-Event"].ToString();
            (string state, List<string> workItemIds) = _gitHubService.GetEventInformation(xGithubEvent, content.ToString());

            foreach (var workItemId in workItemIds)
            {
                SetItemStatus(Organization, Project, Convert.ToInt32(workItemId), state);
            }

            if (xGithubEvent == "pull_request")
            {
                PostMessage("general", $"New pull request - https://github.com/sidkcr/DevOpsSync/pulls");
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