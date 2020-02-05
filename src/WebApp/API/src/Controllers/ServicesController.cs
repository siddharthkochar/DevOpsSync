using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevOpsSync.WebApp.API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevOpsSync.WebApp.API.Controllers
{
    [Route("[controller]")]
    public class ServicesController : ControllerBase
    {
        private readonly DevOpsSyncDbContext _dbContext;

        public ServicesController(DevOpsSyncDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var services = await _dbContext.Services.ToListAsync();

            var servicesViewModels = services.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Color = x.Color
            });

            return Ok(servicesViewModels);
        }

        [HttpGet("{serviceId}/triggers")]
        public async Task<IActionResult> Get([FromRoute] int serviceId)
        {

            var service = await _dbContext.Services.FirstOrDefaultAsync(x => x.Id == serviceId);

            if (service == null)
            {
                return NoContent();
            }

            var triggersViewModels = service.Triggers.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                ServiceId = x.ServiceId
            });

            return Ok(triggersViewModels);
        }

        [HttpGet("{serviceId}/triggers/{triggerId}/services")]
        public IActionResult GetApplicableServicesForTrigger([FromRoute] int serviceId, [FromRoute] int triggerId)
        {
            var actionServices = _dbContext.ServiceTriggerAction
                .Where(x => x.TriggerId == triggerId)
                .Select(x => x.ServiceAction.Service);

            var triggersViewModels = actionServices.Select(x => new
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Color = x.Color
            });

            return Ok(triggersViewModels);
        }

        [HttpGet("{serviceId}/triggers/{triggerId}/services/{actionServiceId}/actions")]
        public IActionResult GetActions([FromRoute] int serviceId, [FromRoute] int triggerId, [FromRoute] int actionServiceId)
        {
            var actions = _dbContext.ServiceTriggerAction
                .Where(x => x.TriggerId == triggerId && x.ServiceAction.ServiceId == actionServiceId)
                .Select(x => x.ServiceAction);

            var actionViewModels = actions.Select(x => new
            {
                Id = x.Id,
                Name = x.Name
            });

            return Ok(actionViewModels);
        }
    }
}
