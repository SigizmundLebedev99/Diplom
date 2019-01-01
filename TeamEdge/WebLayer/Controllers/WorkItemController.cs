using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Route("api/workitems")]
    [Authorize]
    public class WorkItemController : Controller
    {
        readonly IWorkItemService _workItemService;

        public WorkItemController(IWorkItemService service)
        {
            _workItemService = service;
        }

        [HttpGet("project/{projectId}/item")]
        public async Task<IActionResult> GetWorkItem(int projectId, [FromQuery]string code, [FromQuery]int number)
        {
            var result = await _workItemService.GetWorkItem(projectId, User.Id(), code, number);
            return Ok(result);
        } 
        
        [HttpPost]
        public async Task<IActionResult> CreateWorkItem([FromBody][JsonConverter(typeof(WorkItemConverter))]CreateWorkItemDTO model)
        {
            await _workItemService.CreateWorkItem(model);
            return Ok();
        }
    }
}
