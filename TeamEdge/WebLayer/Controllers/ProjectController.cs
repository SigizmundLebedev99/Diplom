using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Route("api/account")]
    [Authorize]
    public class ProjectController : Controller
    {
        public async Task<IActionResult> GetProjectsForUser()
        {
            var result = new ProjectsForUserDTO();
            return Ok(result);
        }

        public async Task<IActionResult> CreateProject(CreateProjectDTO model)
        {
            return Ok();
        }

        public async Task<IActionResult> GetProjectInfo()
        {
            return Ok();
        }

        public async Task<IActionResult> UpdateProjectInfo()
        {
            return Ok();
        }
    }
}
