using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevOpsSync.WebApp.API.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(new[]
            {
                new
                {
                    Id = 1,
                    Name = "GitHub",
                    ImageUrl = "https://assets.ifttt.com/images/channels/2107379463/icons/monochrome_large.png",
                    Color = "#00aeef"
                },
                new
                {
                    Id = 2,
                    Name = "Visual Studio Team Service",
                    ImageUrl = "",
                    Color = "#0da778"
                }
            });
        }

        [HttpGet("{serviceId}/triggers")]
        public async Task<IActionResult> Get([FromRoute] int serviceId)
        {
            return Ok(new[]
            {
                new
                {
                    Id = 1,
                    Name = "Push",
                    Description = "When something is pushed on",
                    ImageUrl = ""
                }
            });
        }
    }
}
