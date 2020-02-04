using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using DevOpsSync.WebApp.API.Services.Slack;
using DevOpsSync.WebApp.Utility;

namespace DevOpsSync.WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlackController : ControllerBase
    {
        private readonly ClientSettings config;
        private IDataStore dataStore;

        public SlackController(IOptions<Settings> config, IDataStore dataStore)

        {
            this.config = config.Value.Slack;
            this.dataStore = dataStore;
        }

        [HttpGet("initialize")]
        public IActionResult Initialize()
        {
            var state = Guid.NewGuid().ToString();
            Response.Cookies.Append("state", state);

            var authorizeUrl = $"https://slack.com/oauth/v2/authorize?" +
                "scope=chat:write,channels:read,im:read,users:read,users:read.email&" +
                $"client_id={config.ClientId}&" +
                $"redirect_uri={config.RedirectUrl}";

            return Redirect(authorizeUrl);
        }

        [HttpGet("auth")]
        public void Auth([FromQuery] string code, [FromQuery] string state)
        {
            var cookies = Request.Cookies["state"];
            if (cookies != state)
            {
                Unauthorized();
            }

            dataStore.Storage[Constants.SlackKey] =
                new SlackService(config.ClientId, config.ClientSecret, code);
        }
    }
}