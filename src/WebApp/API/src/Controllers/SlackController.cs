using DevOpsSync.WebApp.API.Code;
using DevOpsSync.WebApp.API.Models.Slack;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DevOpsSync.WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlackController : ControllerBase
    {
        private readonly ClientSettings config;
        private readonly ISlack slack;

        public SlackController(IOptions<Settings> config, ISlack slack)
        {
            this.config = config.Value.Slack;
            this.slack = slack;
        }

        [HttpGet]
        public string GetConsentUrl()
        {
            var state = Guid.NewGuid().ToString();
            Response.Cookies.Append("state", state);

            return $"https://slack.com/oauth/v2/authorize?" +
            "scope=chat:write,channels:read,im:read,users:read,users:read.email&" +
            $"client_id={config.ClientId}&" +
            $"redirect_uri={config.RedirectUrl}";
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

            var token = await slack.GetTokenAsync(authRequest);
            //TODO: Save token to database
        }
    }
}