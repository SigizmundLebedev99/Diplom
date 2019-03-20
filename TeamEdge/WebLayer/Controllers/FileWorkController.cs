using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        readonly IContentTypeProvider _provider;
        public FileWorkController(IFileWorkService service, FileSystemService systemService, IContentTypeProvider provider)
        {
            _fileWorkService = service;
            _systemService = systemService;
            _provider = provider;
        }

        [HttpGet("file/{fileId}")]
        public async Task<IActionResult> GetFile(int fileId)
        {
            var file = await _fileWorkService.GetFile(fileId, User.Id());
            if (_provider.TryGetContentType(file.name, out var contentType))
                return File(file.bytes, contentType, file.name);
            return BadRequest("Can't define content type");
        }

        [HttpPost("file/project/{projectId}")]
        public async Task<IActionResult> CreateFile(IFormFile file, [FromRoute]int projectId)
        {
            if (file == null || file.Length == 0)
                return BadRequest();
            var imageTypes = new string[]{"image/jpg",
            "image/jpeg",
            "image/pjpeg",
            "image/gif",
            "image/x-png",
            "image/png" };
            FileDTO result = null;
            var dto = new CreateFileDTO
            {
                File = file,
                UserId = User.Id(),
                ProjectId = projectId
            };
            if (imageTypes.Contains(file.ContentType.ToLower()))
                result = await _fileWorkService.CreateImage(dto);
            else
                result = await _fileWorkService.CreateFile(dto); 
            

            result.CreatedBy = new UserLightDTO
            {
                Avatar = User.Avatar(),
                Id = User.Id(),
                Name = User.FullName()
            };

            return Ok(result);
        }

        [HttpPost("file/image")]
        public async Task<IActionResult> SaveAvatar(IFormFile file)
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
    } 
}
