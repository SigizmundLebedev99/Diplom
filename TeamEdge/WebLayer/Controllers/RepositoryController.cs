using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Authorize]
    [Route("api/repo/")]
    public class RepositoryController : Controller
    {
        readonly IRepositoryService _repositoryService;
        readonly IMapper _mapper;

        public RepositoryController(IRepositoryService service, IMapper mapper)
        {
            _mapper = mapper;
            _repositoryService = service;
        }
        /// <summary>
        /// Create repo for current project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpPost("project/{projectId}")]
        public async Task<IActionResult> CreateRepository(int projectId)
        {
            var id = await _repositoryService.CreateRepository(new CreateRepositoryDTO
            {
                ProjectId = projectId,
                UserId = User.Id()
            });
            return Ok(id);
        }

        /// <summary>
        /// Get list of branches to repo
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetBranches(int projectId, [FromQuery]GetBranchesVM model)
        {
            var dto = _mapper.Map<GetBranchesDTO>(model);
            dto.UserId = User.Id();
            dto.ProjectId = projectId;
            var result = await _repositoryService.GetBranches(dto);
            return Ok(result);
        }

        /// <summary>
        /// Create new branch for repo
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("branch")]
        public async Task<IActionResult> CreateBranch([FromBody]CreateBranchVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var dto = _mapper.Map<CreateBranchDTO>(model);
            dto.UserId = User.Id();
            await _repositoryService.CreateBranch(dto);
            return Ok();
        }
    }
}
