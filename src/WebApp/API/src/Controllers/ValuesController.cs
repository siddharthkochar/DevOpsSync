using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet(""), Authorize]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(new List<string> {
                "hey",
                "what"
            });
        }
    }
}