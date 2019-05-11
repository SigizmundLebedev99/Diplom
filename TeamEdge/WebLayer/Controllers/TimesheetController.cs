using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Services;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Authorize]
    [Route("api/timesheet")]
    public class TimesheetController : Controller
    {
        readonly TimesheetService _service;

        public TimesheetController(TimesheetService service)
        {
            _service = service;
        }

        [HttpGet("workitem/{id}")]
        public async Task<IActionResult> GetTimesheet(int id)
        {
            var res = await _service.GetTimesheets(id);
            return Ok(res);
        }

        [HttpPost("status")]
        public async Task<IActionResult> ChangeStatus([FromBody]ChangeWorkItemStatusDTO model)
        {
            model.UserId = User.Id();
            await _service.ChangeStatus(model);
            return Ok();
        }
    }
}
