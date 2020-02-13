using Microsoft.AspNetCore.Mvc;
using System;
using DevOpsSync.WebApp.Services;
using System.Threading.Tasks;

namespace DevOpsSync.WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlackController : ControllerBase
    {
        private readonly ISlackService _slackService;

        public SlackController(ISlackService slackService)

        {
            _slackService = slackService;
        }

        [HttpGet("initialize")]
        public IActionResult Initialize()
        {
            var state = Guid.NewGuid().ToString();
            Response.Cookies.Append("slack-state", state);
            var authorizeUrl = _slackService.GetConsentUrl(state);
            return Redirect(authorizeUrl);
        }

        [HttpGet("auth")]
        public async Task<IActionResult> Auth([FromQuery] string code, [FromQuery] string state)
        {
            var cookies = Request.Cookies["slack-state"];
            if (cookies != state)
            {
                BadRequest();
            }

            var accessToken = await _slackService.GetAccessTokenAsync(code);
            Response.Cookies.Append("slack-token", accessToken);
            return Redirect("http://localhost:3000/services/action/slack");
        }
    }
}