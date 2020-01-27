using DevOpsSync.WebApp.API.Code;
using DevOpsSync.WebApp.API.Models.GitHub;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DevOpsSync.WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GitHubController : ControllerBase
    {
        private readonly GitHubSettings config;
        private readonly IGitHub gitHub;

        public GitHubController(IOptions<Settings> config, IGitHub gitHub)
        {
            this.config = config.Value.GitHub;
            this.gitHub = gitHub;
        }

        [HttpGet]
        public string GetConsentUrl()
        {
            var state = Guid.NewGuid().ToString();
            Response.Cookies.Append("state", state);
            
            return $"https://github.com/login/oauth/authorize" +
              $"?client_id={config.ClientId}" +
              $"&redirect_uri={config.RedirectUrl}" +
              $"&state={state}";
        }

        [HttpGet("auth")]
        public async Task Auth([FromQuery] string code, [FromQuery] string state)
        {
            var cookies = Request.Cookies["state"];
            if(cookies != state)
            {
                Unauthorized();
            }

            var authRequest = new Token.Request
            {
                ClientId = config.ClientId,
                CilentSecret = config.ClientSecret,
                Code = code
            };

            var token = await gitHub.GetTokenAsync(authRequest);
            //TODO: Save token to database
        }
    }
}