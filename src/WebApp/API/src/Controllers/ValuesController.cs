using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{
    [Route("[controller]")]
    public class ValuesController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(new List<string> {
                "hey",
                "what"
            });
        }
    }
}