using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DevOpsSync.WebApp.API.Services.VSTS;

namespace DevOpsSync.WebApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VSTSController : ControllerBase
    {
        private readonly ClientSettings config;
        private readonly IDataStore dataStore;

        public VSTSController(
                    IOptions<Settings> config,
                    IDataStore dataStore)
        {
            this.config = config.Value.VSTS;
            this.dataStore = dataStore;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate()
        {
            return Redirect(VSTSService.GetAuthUrl(config.ClientId, config.RedirectUrl));
        }

        [HttpGet("initialize")]
        public IActionResult Initialize([FromQuery] string code)
        {
            dataStore.ObjectStorage[Constants.VSTSServiceKey] =
                new VSTSService(config.ClientSecret, config.RedirectUrl, code);
            return Ok();
        }
    }
}