using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Route("api/file")]
    [Authorize]
    public class FileWorkController : Controller
    {
        readonly IFileWorkService _fileWorkService;

        public FileWorkController(IFileWorkService service)
        {
            _fileWorkService = service;
        }

        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetFile(int fileId)
        {
            var file = await _fileWorkService.GetFile(fileId, User.Id());
            return File(file.bytes, file.type, file.name);
        }

        [HttpPost("{projectId}")]
        public async Task<IActionResult> CreateFile(IFormFile file, [FromRoute]int projectId)
        {
            if (file == null || file.Length == 0)
                return BadRequest();
            int id = await _fileWorkService.CreateFile(new CreateFileDTO
            {
                File = file,
                UserId = User.Id(),
                ProjectId = projectId
            });

            return Ok(new FileDTO
            {
                Id = id,
                CreatedBy = new UserLightDTO
                {
                    Avatar = User.Avatar(),
                    Id = User.Id(),
                    Name = User.FullName()
                },
                FileName = file.FileName
            });
        }
    }
}
