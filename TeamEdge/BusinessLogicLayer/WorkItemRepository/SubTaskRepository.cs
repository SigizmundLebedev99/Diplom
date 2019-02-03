using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class SubTaskRepository : WorkItemRepository
    {
        public SubTaskRepository(IServiceProvider provider) : base(provider) { }

        public override async Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            var entity = _mapper.Map<SubTask>(model);

            if (model.ParentId != null)
                operRes.Plus(await CheckParent<_Task>(model.ProjectId, model.ParentId.Value));

            if (!operRes.Succeded)
                return operRes;
            entity.Number = await GetNumber<SubTask>(model.ProjectId);
            entity.DescriptionId = description.Id;
            _context.SubTasks.Add(entity);

            await _context.SaveChangesAsync();
            operRes.Result = await _context.SubTasks.Select(SelectExpression).FirstOrDefaultAsync(e => e.DescriptionId == description.Id);
            return operRes;
        }

        public override Task<WorkItemDTO> GetWorkItem(string code, int number, int project)
        {
            return _context.SubTasks.Where(e => e.Description.ProjectId == project && e.Number == number)
               .Select(SelectExpression).FirstOrDefaultAsync();
        }

        public override IQueryable<ItemDTO> GetItems(GetItemsDTO model)
        {
            var filter = WorkItemHelper.GetFilter<SubTask>(model);
            return _context.SubTasks.Where(filter).Select(WorkItemHelper.ItemDTOSelector);
        }

        public override async Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int number, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);

            var nextentity = _mapper.Map<SubTask>(model);
            var nextdesc = _mapper.Map<WorkItemDescription>(model);

            var query = _context.SubTasks
                .Include(e => e.Description).ThenInclude(e => e.Files).ThenInclude(e => e.File)
                .Include(e => e.Description).ThenInclude(e => e.Branches);

            var entity = await query
                .FirstOrDefaultAsync(e => e.Description.ProjectId == model.ProjectId && e.Number == number);

            if (entity == null)
                throw new NotFoundException("item_nf");
            if (nextentity.AssignedToId != null && nextentity.AssignedToId.Value != model.CreatorId)
                if (!await _context.UserProjects.AnyAsync(e => e.UserId == model.CreatorId && e.ProjectId == model.ProjectId))
                    throw new UnauthorizedException("Назначать участников на задачу может только администратор");
            nextentity.DescriptionId = entity.DescriptionId;
            nextentity.Number = entity.Number;
            WorkItemHelper.RestoreDescriptionData(entity.Description, nextdesc);

            if (model.ParentId != null)
                operRes.Plus(await CheckParent<_Task>(model.ProjectId, model.ParentId.Value));

            if (!operRes.Succeded)
                return operRes;

            var files = nextdesc.Files;
            nextdesc.Files = null;
            var tags = nextdesc.Tags;
            nextdesc.Tags = null;
            DetachAllEntities(entity);
            _context.WorkItemDescriptions.Update(nextdesc);
            UpdateFiles(entity.Description.Files, files, nextdesc.Id);
            UpdateTags(entity.Description.Tags, tags);
            _context.SubTasks.Update(nextentity);

            await _context.SaveChangesAsync();

            var result = await query.Where(e => e.DescriptionId == entity.DescriptionId).ToListAsync();
            operRes.Result = result.Select(SelectExpression.Compile()).First();
            _historyService.CompareForChanges(entity, result.First(), _httpContext.User);
            return operRes;
        }

        private static readonly Expression<Func<SubTask, WorkItemDTO>> SelectExpression = e => new TaskInfoDTO
        {
            AssignedTo = e.AssignedToId == null ? null : new UserDTO
            {
                Avatar = e.AssignedTo.Avatar,
                Email = e.AssignedTo.Email,
                FullName = e.AssignedTo.FullName,
                Id = e.AssignedToId.Value,
                UserName = e.AssignedTo.UserName
            },
            Code = WorkItemType.SubTask.Code(),
            DescriptionId = e.DescriptionId,
            Name = e.Name,
            Number = e.Number,
            Status = e.Status,
            Parent = e.Parent == null ? null : new ItemDTO
            {
                Code = e.Parent.Type.Code(),
                Name = e.Parent.Name,
                Number = e.Parent.Number,
                DescriptionId = e.Parent.DescriptionId
            },
            Description = new DescriptionDTO
            {
                CreatedBy = new UserDTO
                {
                    Avatar = e.Description.Creator.Avatar,
                    Email = e.Description.Creator.Email,
                    FullName = e.Description.Creator.FullName,
                    Id = e.Description.CreatorId,
                    UserName = e.Description.Creator.UserName
                },
                DateOfCreation = e.Description.DateOfCreation,
                Description = e.Description.DescriptionText,
                LastUpdate = e.Description.LastUpdate,
                LastUpdateBy = e.Description.LastUpdaterId == null ? null : new UserDTO
                {
                    Avatar = e.Description.LastUpdater.Avatar,
                    Email = e.Description.LastUpdater.Email,
                    FullName = e.Description.LastUpdater.FullName,
                    Id = e.Description.LastUpdaterId.Value,
                    UserName = e.Description.LastUpdater.UserName
                }
            }
        };
    }
}
