using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Authorize]
    public class RepositoryController : Controller
    {
        readonly IRepositoryService _repositoryService;

        public RepositoryController(IRepositoryService service)
        {
            _repositoryService = service;
        }
        /// <summary>
        /// Create repo for current project
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<IActionResult> CreateRepository(int projectId)
        {
            var id = await _repositoryService.CreateRepository(new CreateRepositoryDTO
            {
                ProjectId = projectId,
                UserId = User.Id()
            });
            return Ok(id);
        }
    }
}
