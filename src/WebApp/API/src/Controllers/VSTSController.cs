using Microsoft.AspNetCore.Mvc;
using DevOpsSync.WebApp.Services;
using System;
using System.Threading.Tasks;

namespace DevOpsSync.WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VSTSController : ControllerBase
    {
        private readonly IDevOpsService _devOpsService;

        public VSTSController(IDevOpsService devOpsService)
        {
            _devOpsService = devOpsService;
        }

        [HttpGet("initialize")]
        public IActionResult Initialize()
        {
            var state = Guid.NewGuid().ToString();
            Response.Cookies.Append("devops-state", state);
            var authorizeUrl = _devOpsService.GetConsentUrl(state);
            return Redirect(authorizeUrl);
        }

        [HttpGet("auth")]
        public async Task Auth([FromQuery] string code, [FromQuery] string state)
        {
            var cookies = Request.Cookies["devops-state"];
            if (cookies != state)
            {
                BadRequest();
            }

            await _devOpsService.SetRefreshTokenAsync(code);
        }
    }
}