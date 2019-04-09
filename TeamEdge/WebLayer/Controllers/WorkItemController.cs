using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Route("api/workitems")]
    [Authorize]
    public class WorkItemController : Controller
    {
        readonly IWorkItemService _workItemService;
        readonly IMapper _mapper;

        public WorkItemController(IWorkItemService service, IMapper mapper)
        {
            _workItemService = service;
            _mapper = mapper;
        }

        [HttpGet("project/{projectId}/item/{code}/{number}")]
        public async Task<IActionResult> GetWorkItem(int projectId, string code, int number)
        {
            var result = await _workItemService.GetWorkItem(projectId, User.Id(), code, number);
            return Ok(result);
        } 
        
        [HttpPost]
        public async Task<IActionResult> CreateWorkItem([FromBody]CreateWorkItemDTO model)
        {
            model.CreatorId = User.Id();
            var res = await _workItemService.CreateWorkItem(model);
            return res.GetResult();
        }

        [HttpGet("project/{projectId}/items")]
        public async Task<IActionResult> GetListOfItems(int projectId, GetItemsVM model)
        {
            var dto = _mapper.Map<GetItemsDTO>(model);
            dto.ProjectId = projectId;
            dto.UserId = User.Id();
            var result = await _workItemService.GetListOfItems(dto);
            return Ok(result);
        }

        [HttpGet("project/{projectId}/backlog")]
        public async Task<IActionResult> GetBacklog(int projectId)
        {
            var result = await _workItemService.GetBacklog(projectId);
            return Ok(result);
        }

        [HttpPut("number/{number}")]
        public async Task<IActionResult> UpdateWorkItem(int number, [FromBody]CreateWorkItemDTO model)
        {
            model.CreatorId = User.Id();
            var res = await _workItemService.UpdateWorkItem(number, model);
            return res.GetResult();
        }

        [HttpGet("project/{projectId}/user/{userId}")]
        public async Task<IActionResult> GetTasksForUser(int projectId, int userId)
        {
            var res = await _workItemService.GetTasksForUser(projectId, userId);
            return Ok(res);
        }

        [HttpGet("project/{projectId}/itemdense/{code}/{number}")]
        public async Task<IActionResult> GetDenseWorkItem(int projectId, string code, int number)
        {
            var res = await _workItemService.GetDenseWorkItem(code, number, projectId, User.Id());
            return Ok(res);
        }
    }
}
