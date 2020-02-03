using DevOpsSync.WebApp.API.Code;
using DevOpsSync.WebApp.API.Models.Slack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using DevOpsSync.WebApp.API.Services.Slack;

namespace DevOpsSync.WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlackController : ControllerBase
    {
        private readonly ClientSettings config;
        private readonly ISlack slack;
        private IDataStore dataStore;

        public SlackController(IOptions<Settings> config, ISlack slack, IDataStore dataStore)
)
        {
            this.config = config.Value.Slack;
            this.slack = slack;
            this.dataStore = dataStore;
        }

        [HttpGet]
        public string GetConsentUrl()
        {
            var state = Guid.NewGuid().ToString();
            Response.Cookies.Append("state", state);

            Redirect($"https://slack.com/oauth/v2/authorize?" +
            "scope=chat:write,channels:read,im:read,users:read,users:read.email&" +
            $"client_id={config.ClientId}&" +
            $"redirect_uri={config.RedirectUrl}");
        }

        [HttpGet("auth")]
        public async Task Auth([FromQuery] string code, [FromQuery] string state)
        {
            var cookies = Request.Cookies["state"];
            if (cookies != state)
            {
                Unauthorized();
            }

            var authRequest = new Token.Request
            {
                ClientId = config.ClientId,
                CilentSecret = config.ClientSecret,
                Code = code
            };

            dataStore.Storage[Constants.SlackKey] =
                new SlackService(config.ClientId, config.ClientSecret, code);
        }
    }
}