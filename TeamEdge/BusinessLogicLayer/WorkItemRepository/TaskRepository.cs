using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class TaskRepository : WorkItemRepository
    {
        public TaskRepository(IServiceProvider provider) : base(provider) { }

        public override Task<WorkItemDTO> GetWorkItem(string code, int number, int project)
        {
            TaskType type = Enum.Parse<TaskType>(WorkItemFactory.GetEnumElement(code));
            return _context.Tasks
                .Where(e => e.Type == type && e.Number == number && e.Description.ProjectId == project)
                .Select(SelectExpression)
                .FirstOrDefaultAsync();
        }

        public override async Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            var entity = _mapper.Map<_Task>(model);

            var checkResult = await CheckChildren<_Task>(model.ChildrenIds, model.ProjectId);
            operRes.Plus(checkResult);
            operRes.Plus(CheckStatus(checkResult.Result, entity.Status));
            if (model.ParentId != null)
                operRes.Plus(await CheckParent<UserStory>(model.ProjectId, model.ParentId.Value));

            if (!operRes.Succeded)
                return operRes;
            
            var children = checkResult.Result;

            entity.Type = Enum.Parse<TaskType>(WorkItemFactory.GetEnumElement(model.Code));
            entity.Number = await GetNumber<_Task>(model.ProjectId, t=>t.Type == entity.Type);
            entity.DescriptionId = description.Id;

            if (children != null)
            {
                foreach (var t in children)
                    t.ParentId = entity.DescriptionId;
                _context.Tasks.UpdateRange(children);
            }

            _context.Tasks.Add(entity);

            await _context.SaveChangesAsync();
            operRes.Result = await _context.Tasks.Select(SelectExpression).FirstOrDefaultAsync(e => e.DescriptionId == description.Id);
            return operRes;
        }

        public override IQueryable<ItemDTO> GetItems(GetItemsDTO model)
        {
            var type = Enum.Parse<TaskType>(WorkItemFactory.GetEnumElement(model.Code));
            var filter = WorkItemHelper.GetFilter<_Task>(model);
            return _context.Tasks.Where(e=>e.Type == type).Where(filter).Select(WorkItemHelper.ItemDTOSelector);
        }

        public override async Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int number, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            var type = Enum.Parse<TaskType>(WorkItemFactory.GetEnumElement(model.Code));
            var nextentity = _mapper.Map<_Task>(model);
            var nextdesc = _mapper.Map<WorkItemDescription>(model);

            var query = _context.Tasks
                .Include(e => e.Description).ThenInclude(e => e.Files).ThenInclude(e => e.File)
                .Include(e => e.Description).ThenInclude(e => e.Branches)
                .Include(e=> e.AssignedTo)
                .Include(e => e.Parent);

            var entity = await query
                .FirstOrDefaultAsync(e => e.Description.ProjectId == model.ProjectId && e.Number == number && e.Type == type);
           
            if (entity == null)
                throw new NotFoundException("item_nf");

            if (nextentity.AssignedToId != null && nextentity.AssignedToId.Value != model.CreatorId)
                if (!await _context.UserProjects.AnyAsync(e => e.UserId == model.CreatorId && e.ProjectId == model.ProjectId))
                    throw new UnauthorizedException("Назначать участников на задачу может только администратор");

            nextentity.DescriptionId = entity.DescriptionId;
            nextentity.Type = type;
            nextentity.Number = entity.Number;
            WorkItemHelper.RestoreDescriptionData(entity.Description, nextdesc);

            var checkResult = await CheckChildren<SubTask>(model.ChildrenIds, model.ProjectId);
            operRes.Plus(checkResult);
            operRes.Plus(CheckStatus(checkResult.Result, entity.Status));
            if (model.ParentId != null)
                operRes.Plus(await CheckParent<UserStory>(model.ProjectId, model.ParentId.Value));

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
            UpdateChildren<SubTask, _Task>(entity.Children, checkResult.Result, entity.DescriptionId);
            _context.Tasks.Update(nextentity);

            await _context.SaveChangesAsync();

            var result = await query.Where(e => e.DescriptionId == entity.DescriptionId).ToListAsync();
            operRes.Result = result.Select(SelectExpression.Compile()).First();
            _historyService.CompareForChanges(entity, result.First(), _httpContext.User);
            return operRes;
        }

        private static readonly Expression<Func<_Task, WorkItemDTO>> SelectExpression = e => new TaskInfoDTO
        {
            AssignedTo = e.AssignedToId == null? null : new UserDTO
            {
                Avatar = e.AssignedTo.Avatar,
                Email = e.AssignedTo.Email,
                FullName = e.AssignedTo.FullName,
                Id = e.AssignedToId.Value,
                UserName = e.AssignedTo.UserName
            },
            Code = e.Type.Code(),
            DescriptionId = e.DescriptionId,         
            Name = e.Name,
            Number = e.Number,
            Status = e.Status.ToString(),
            Children = e.Children.Select(a => new ItemDTO
            {
                Code = WorkItemType.SubTask.Code(),
                Name = a.Name,
                Number = a.Number,
                DescriptionId = a.DescriptionId
            }),
            Parent = e.Parent == null ? null : new ItemDTO
            {
                Code = WorkItemType.UserStory.Code(),
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
