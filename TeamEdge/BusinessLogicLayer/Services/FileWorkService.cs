using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class FileWorkService : IFileWorkService
    {
        readonly TeamEdgeDbContext _context;
        readonly IHostingEnvironment _env;
        readonly FileSystemService _fileSystemService;
        readonly IValidationService _validationService;
        public FileWorkService(TeamEdgeDbContext context, IHostingEnvironment env, FileSystemService fileSystemService, IValidationService validationService)
        {
            _validationService = validationService;
            _context = context;
            _env = env;
            _fileSystemService = fileSystemService;
        }

        public async Task<FileDTO> CreateFile(CreateFileDTO model)
        {
            await _validationService.ValidateProjectAccess(model.ProjectId, model.UserId);
            string path = await _fileSystemService.DocsSave(model.File);
            return await CreateFile(model, path, false);
        }

        public async Task<FileDTO> CreateImage(CreateFileDTO model)
        {
            await _validationService.ValidateProjectAccess(model.ProjectId, model.UserId);
            string path = await _fileSystemService.ImageSave(model.File);
            return await CreateFile(model, path, true);
        }

        private async Task<FileDTO> CreateFile(CreateFileDTO model, string path, bool isPicture)
        {
            if (path == null)
                throw new InvalidOperationException();
            if (!isPicture)
            {
                var existingFile = await _context.Files
                    .Where(e => e.Path == path && e.ProjectId == model.ProjectId)
                    .Select(Selector)
                    .FirstOrDefaultAsync();

                if (existingFile != null)
                    return existingFile;
            }

            var file = new _File
            {
                CreatorId = model.UserId,
                DateOfCreation = DateTime.Now,
                ProjectId = model.ProjectId,
                FileName = model.File.FileName,
                Path = path,
                IsPicture = isPicture
            };
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
            return new FileDTO()
            {
                Id = file.Id,
                DateOfCreation = file.DateOfCreation,
                FileName = file.FileName,
                IsPicture = isPicture,
                Path = isPicture ? file.Path : null
            };
        }

        public async Task<(byte[],string,string)> GetFile(int fileId, int userId)
        {
            var file = await _context.Files.Where(e => e.Id == fileId)
                .Select(e => new { e.Path, e.FileName, e.IsPicture})
                .FirstOrDefaultAsync();
            if (file == null)
                throw new NotFoundException("file_nf");
            if(!await _context.Files
                .Where(e=>e.Id == fileId)
                .AnyAsync(e => e.Project
                    .Users
                    .Select(u => u.UserId)
                    .Contains(userId)))
                throw new UnauthorizedException();
            if (!file.IsPicture)
            {
                string path = Path.Combine(_env.ContentRootPath, file.Path);
                if (File.Exists(path))
                {
                    FileInfo info = new FileInfo(path);
                    return (await File.ReadAllBytesAsync(path), info.Extension, file.FileName);
                }
                throw new NotFoundException("file_nf", "Не удалось найти файл по пути " + path);
            }
            else
            {
                var path = file.Path.Substring(file.Path.IndexOf("base64, ") + 8);
                var bytes = Convert.FromBase64String(path);
                return (bytes, file.FileName.Split('.').Last(), file.FileName);
            }
        }

        public async Task<IEnumerable<FileDTO>> GetFilesForProject(int userId, int projectId)
        {
            await _validationService.ValidateProjectAccess(projectId, userId);
            return await _context.Files.Where(e => e.ProjectId == projectId).Select(Selector).ToArrayAsync();
        }   

        //public async Task DeleteFile(int userId, int fileId)
        //{
        //    var file = await _context.Files.FirstOrDefaultAsync(e => e.Id == fileId);
        //    await _validationService.ValidateProjectAccess(file.ProjectId, userId);
        //    _fileSystemService.RemoveFile(file.Path, file.IsPicture);
        //    _context.Files.Remove(file);
        //    await _context.SaveChangesAsync();
        //}

        public async Task<IEnumerable<FileDTO>> GetFilesForItem(int itemId, int userId, int projectId)
        {
            await _validationService.ValidateProjectAccess(projectId, userId);
            var files = await _context.WorkItemFiles.Where(e => e.WorkItemId == itemId).Select(e => e.File).Select(Selector).ToListAsync();
            return files;
        }

        private Expression<Func<_File, FileDTO>> Selector = e => 
        new FileDTO
        {
            Id = e.Id,
            FileName = e.FileName,
            CreatedBy = new UserLightDTO
            {
                Avatar = e.Creator.Avatar,
                Name = e.Creator.FullName,
                Id = e.CreatorId
            },
            DateOfCreation = e.DateOfCreation,
            IsPicture = e.IsPicture,
            Path = e.IsPicture?e.Path:null
        };
    }
}
