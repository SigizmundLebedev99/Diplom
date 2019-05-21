using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Route("api/sprints")]
    [Authorize]
    public class SprintController : Controller
    {
        readonly IMapper _mapper;
        readonly ISprintService _sprintService;

        public SprintController(IMapper mapper, ISprintService sprintService)
        {
            _mapper = mapper;
            _sprintService = sprintService;
        }

        /// <summary>
        /// Создать новый спринт
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("project/{projectId}")]
        public async Task<IActionResult> CreateSprint(int projectId, [FromBody]CreateSprintVM model)
        {
            var dto = _mapper.Map<CreateSprintDTO>(model);
            dto.CreatorId = User.Id();
            dto.ProjectId = projectId;
            var res = await _sprintService.CreateSprint(dto);
            return Ok(res);
        }

        /// <summary>
        /// Изменить план спринта
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="number"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("project/{projectId}/sprint/{number}")]
        public async Task<IActionResult> UpdateSprint(int projectId, int number, [FromBody]CreateSprintVM model)
        {
            var dto = _mapper.Map<UpdateSprintDTO>(model);
            dto.Number = number;
            dto.ProjectId = projectId;
            dto.UserId = User.Id();
            await _sprintService.UpdateSprint(dto);
            return Ok();
        }

        /// <summary>
        /// Получить список спринтов
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetSprints(int projectId)
        {
            var res = await _sprintService.GetSprintsForProject(User.Id(), projectId);
            return Ok(res);
        }
    }
}
