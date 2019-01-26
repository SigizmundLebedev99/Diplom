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
        readonly PathParams _parameters;
        readonly FileSystemService _fileSystemService;
        readonly IValidationService _validationService;

        public FileWorkService(TeamEdgeDbContext context, PathParams parameters, FileSystemService fileSystemService, IValidationService validationService)
        {
            _validationService = validationService;
            _context = context;
            _parameters = parameters;
            _fileSystemService = fileSystemService;
        }

        public async Task<FileDTO> CreateFile(CreateFileDTO model)
        {
            await _validationService.ValidateProject(model.ProjectId, model.UserId);

            string path = await _fileSystemService.DocsSave(model.File);
            if (path == null)
                throw new InvalidOperationException();
            var existingFile = await _context.Files
                .Where(e => e.FilePath == path && e.ProjectId == model.ProjectId)
                .Select(Selector)
                .FirstOrDefaultAsync();

            if (existingFile != null)
                return existingFile;

            var file = new _File
            {
                CreatorId = model.UserId,
                DateOfCreation = DateTime.Now,
                ProjectId = model.ProjectId,
                FileName = model.File.FileName,
                FilePath = path
            };
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
            return new FileDTO
            {
                Id = file.Id,
                DateOfCreation = file.DateOfCreation,
                FileName = file.FileName
            };
        }

        public async Task<(byte[],string,string)> GetFile(int fileId, int userId)
        {
            var file = await _context.Files.Where(e => e.Id == fileId)
                .Select(e => new { e.FilePath, e.FileName })
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
            string path = Path.Combine(_parameters.FileDirectoryPath, file.FilePath);
            if (File.Exists(path))
            {
                FileInfo info = new FileInfo(path);
                return (await File.ReadAllBytesAsync(path), info.Extension, file.FileName);
            }
            throw new NotFoundException("file_nf", "Не удалось найти файл по пути " + path);
        }

        public async Task<IEnumerable<FileDTO>> GetFilesForProject(int userId, int projectId)
        {
            await _validationService.ValidateProject(projectId, userId);
            return await _context.Files.Where(e => e.ProjectId == projectId).Select(Selector).ToArrayAsync();
        }   

        public async Task DeleteFile(int userId, int fileId)
        {
            var file = await _context.Files.FirstOrDefaultAsync(e => e.Id == fileId);
            await _validationService.ValidateProject(file.ProjectId, userId, e=>e.CanWrite);
            _fileSystemService.RemoveFile(file.FilePath);
            _context.Files.Remove(file);
            await _context.SaveChangesAsync();
        }

        private Expression<Func<_File, FileDTO>> Selector = e => new FileDTO
        {
            Id = e.Id,
            FileName = e.FileName,
            CreatedBy = new UserLightDTO
            {
                Avatar = e.Creator.Avatar,
                Name = e.Creator.FullName,
                Id = e.CreatorId
            },
            DateOfCreation = e.DateOfCreation
        };
    }
}
