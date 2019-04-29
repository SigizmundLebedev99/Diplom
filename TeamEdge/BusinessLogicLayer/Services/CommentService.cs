using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class CommentService : ICommentService
    {
        readonly TeamEdgeDbContext _context;
        readonly IValidationService _validationService;
        readonly IMapper _mapper;

        public CommentService(TeamEdgeDbContext context, IValidationService validationService, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _validationService = validationService;
        }

        public async Task<OperationResult<int>> CreateComment(CreateCommentDTO model)
        {
            var operRes = new OperationResult<int>(true);
            var project = await _context.WorkItemDescriptions.Where(e => e.Id == model.WorkItemId).Select(e => e.ProjectId).FirstOrDefaultAsync();
            if (project == 0)
                throw new NotFoundException("item_nf");
            await _validationService.ValidateProjectAccess(project, model.From.Id);
            if (model.Files != null && model.Files.Count() > 0)
            {
                operRes.Plus(await _validationService.ValidateFileIds(model.Files, project));
            }
            if (!operRes.Succeded)
                return operRes;
            var comment = _mapper.Map<Comment>(model);
            comment.Files = new List<CommentFile>();
            if (model.Files != null && model.Files.Count() > 0)
            {
                var wiFiles = await _context.WorkItemFiles
                    .Where(e => e.WorkItemId == model.WorkItemId && model.Files.Contains(e.FileId))
                    .Select(e=>e.FileId)
                    .ToListAsync();
                foreach (int f in model.Files.Where(e => !wiFiles.Contains(e)))
                {
                    comment.Files.Add(new CommentFile { FileId = f });
                    _context.WorkItemFiles.Add(new WorkItemFile { FileId = f, WorkItemId = model.WorkItemId });
                }
                foreach (int f in wiFiles)
                    comment.Files.Add(new CommentFile { FileId = f });
            }
            comment.DateOfCreation = DateTime.Now;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            operRes.Result = comment.Id;
            return operRes;
        }

        public async Task<IEnumerable<CommentDTO>> GetComments(int userId, int workItemId)
        {
            var project = await _context.WorkItemDescriptions.Where(e => e.Id == workItemId).Select(e => e.ProjectId).FirstOrDefaultAsync();
            if (project == 0)
                throw new NotFoundException("item_nf");
            await _validationService.ValidateProjectAccess(project, userId);
            return await _context.Comments
                .Where(e => e.WorkItemId == workItemId)
                .Select(e => new CommentDTO
                {
                    DateOfCreation = e.DateOfCreation,
                    Files = e.Files.Select(f=>new FileLightDTO
                    {
                        Id = f.FileId,
                        isPicture = f.File.IsPicture,
                        Name = f.File.FileName,
                        ImageBase64 = f.File.Path
                    }),
                    Text = e.Text,
                    User = new UserLightDTO { Avatar = e.Creator.Avatar,Id = e.CreatorId, Name = e.Creator.FullName }
                }).ToListAsync();
            
        }
    }
}
