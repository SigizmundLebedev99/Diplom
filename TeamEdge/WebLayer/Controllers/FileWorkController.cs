using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.BusinessLogicLayer.Services;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Route("api")]
    [Authorize]
    public class FileWorkController : Controller
    {
        readonly IFileWorkService _fileWorkService;
        readonly FileSystemService _systemService;
        public FileWorkController(IFileWorkService service, FileSystemService systemService)
        {
            _fileWorkService = service;
            _systemService = systemService;
        }

        [HttpGet("file/{fileId}")]
        public async Task<IActionResult> GetFile(int fileId)
        {
            var file = await _fileWorkService.GetFile(fileId, User.Id());
            return File(file.bytes, file.type, file.name);
        }

        [HttpPost("file/project/{projectId}")]
        public async Task<IActionResult> CreateFile(IFormFile file, [FromRoute]int projectId)
        {
            if (file == null || file.Length == 0)
                return BadRequest();
            var result = await _fileWorkService.CreateFile(new CreateFileDTO
            {
                File = file,
                UserId = User.Id(),
                ProjectId = projectId
            });

            if (result.CreatedBy == null)
                result.CreatedBy = new UserLightDTO
                {
                    Avatar = User.Avatar(),
                    Id = User.Id(),
                    Name = User.FullName()
                };

            return Ok(result);
        }

        [HttpPost("file/image")]
        public async Task<IActionResult> SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest();
            var result = await _systemService.AvatarSave(file, User.Id());
            return Ok(result);
        }

        [HttpGet("project/{projectId}/files")]
        public async Task<IActionResult> GetFiles(int projectId)
        {
            var result = await _fileWorkService.GetFilesForProject(User.Id(), projectId);
            return Ok(result);
        }

        [HttpDelete("file/{fileId}")]
        public async Task<IActionResult> DeleteFile(int fileId)
        {
            await _fileWorkService.DeleteFile(User.Id(), fileId);
            return Ok();
        }
    }
}
