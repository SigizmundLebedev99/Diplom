using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.BusinessLogicLayer.Services;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Route("api/project")]
    [Authorize]
    public class ProjectController : Controller
    {
        readonly IProjectService _projectService;
        readonly IMapper _mapper;
        readonly FileSystemService _fsService;

        public ProjectController(IProjectService projectService, IMapper mapper, FileSystemService fsService)
        {
            _projectService = projectService;
            _mapper = mapper;
            _fsService = fsService;
        }

        /// <summary>
        /// Получить список проектов и список инвайтов для пользователя
        /// </summary>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(200, Type = typeof(ProjectsForUserDTO))]
        public async Task<IActionResult> GetProjectsForUser(int userId)
        {
            var result = await _projectService.GetProjectsForUserAsync(userId);
            if (result == null)
                throw new NotFoundException("user_nf");
            return Ok(result);
        }

        /// <summary>
        /// Создвть новый проект
        /// </summary>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ProjectDTO))]
        public async Task<IActionResult> CreateProject([FromBody]CreateProjectVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var dto = _mapper.Map<CreateProjectDTO>(model);
            dto.UserId = User.Id();
            var result = await _projectService.CreateProject(dto);
            _fsService.Commit(dto.UserId, model.Logo);
            return Ok(result);
        }

        /// <summary>
        /// Получить информацию о проекте
        /// </summary>
        [HttpGet("{projectId}")]
        [ProducesResponseType(200, Type = typeof(ProjectInfoDTO))]
        public async Task<IActionResult> GetProjectInfo(int projectId)
        {
            var result = await _projectService.GetProjectInfo(projectId, User.Id());
            return Ok(result);
        }

        /// <summary>
        /// Обновить лого проекта
        /// </summary>
        [HttpPut("{projectId}")]
        [ProducesResponseType(200, Type = typeof(ProjectDTO))]
        public async Task<IActionResult> UpdateProjectInfo(int projectId, [FromForm]CreateProjectVM model)
        {
            var dto = _mapper.Map<CreateProjectDTO>(model);
            dto.UserId = User.Id();
            var result = await _projectService.UpdateProject(projectId, dto);
            _fsService.Commit(dto.UserId, model.Logo);
            return Ok(result);
        }

        /// <summary>
        /// Получить файлы проекта
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("files/{projectId}")]
        public async Task<IActionResult> GetFilesForProject(int projectId)
        {
            var res = await _projectService.GetFilesForProject(projectId, User.Id());
            return Ok(res);
        }

        /// <summary>
        /// Получить инвайты для проекта
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet("invites/{projectId}")]
        public async Task<IActionResult> GetInvitesForProject(int projectId)
        {
            var res = await _projectService.GetInvitesToProject(User.Id(), projectId);
            return Ok(res);
        }
    }
}
