using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class FileWorkService : IFileWorkService
    {
        readonly TeamEdgeDbContext _context;
        readonly PathParams _parameters;
        readonly FileSystemService _fileSystemService;
        readonly IValidationService _validationService;

        public FileWorkService(TeamEdgeDbContext context, PathParams parameters, FileSystemService fileSystemService)
        {
            _context = context;
            _parameters = parameters;
            _fileSystemService = fileSystemService;
        }

        public async Task<int> CreateFile(CreateFileDTO model)
        {
            await _validationService.ValidateProject(model.ProjectId, model.UserId);

            string path = await _fileSystemService.DocsSave(model.File);
            if (path == null)
                throw new InvalidOperationException();
            var existingFile = await _context.Files
                .Where(e => e.FilePath == path && e.ProjectId == model.ProjectId)
                .Select(e => new { e.Id })
                .FirstOrDefaultAsync();

            if (existingFile != null)
                return existingFile.Id;

            var file = new TeamEdge.DAL.Models.File
            {
                CreatorId = model.UserId,
                DateOfCreation = DateTime.Now,
                ProjectId = model.ProjectId,
                FileName = model.File.FileName,
                FilePath = path
            };
            _context.Files.Add(file);
            await _context.SaveChangesAsync();
            return file.Id;
        }

        public async Task<(byte[],string,string)> GetFile(int fileId, int userId)
        {
            var file = await _context.WorkItemFiles.Where(e => e.FileId == fileId)
                .Select(e => new { e.File.FilePath, e.File.FileName })
                .FirstOrDefaultAsync();
            if (file == null)
                throw new NotFoundException("file_nf");
            if(!await _context.WorkItemFiles
                .Where(e=>e.FileId == fileId)
                .AnyAsync(e => e.WorkItem.Project
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
    }
}
