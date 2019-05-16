using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    class TaskRepository : WorkItemRepository
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

        public override Task<ItemDTO> GetDenseWorkItem(string code, int number, int projectId)
        {
            TaskType type = Enum.Parse<TaskType>(WorkItemFactory.GetEnumElement(code));
            return _context.Tasks
                .Where(e => e.Type == type && e.Number == number && e.Description.ProjectId == projectId)
                .Select(WorkItemHelper.ItemDTOSelector)
                .FirstOrDefaultAsync();
        }

        public override async Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model, UserProject userProj = null)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            var entity = _mapper.Map<_Task>(model);

            if (model.ParentId != null)
                operRes.Plus(await CheckParent<UserStory>(model.ProjectId, model.ParentId.Value));

            if (!operRes.Succeded)
                return operRes;

            entity.Type = Enum.Parse<TaskType>(WorkItemFactory.GetEnumElement(model.Code));
            entity.Number = await GetNumber<_Task>(model.ProjectId, t=>t.Type == entity.Type);
            entity.DescriptionId = description.Id;

            _context.Tasks.Add(entity);

            await _context.SaveChangesAsync();
            operRes.Result = await _context.Tasks.Select(SelectExpression).FirstOrDefaultAsync(e => e.DescriptionId == description.Id);
            return operRes;
        }

        public override IQueryable<ItemDTO> GetItems(GetItemsDTO model)
        {
            var filter = WorkItemHelper.GetFilter<_Task>(model);
            if (model.Code.EndsWith('!'))
                return _context.Tasks.Where(filter).Select(WorkItemHelper.ItemDTOSelector);

            var type = Enum.Parse<TaskType>(WorkItemFactory.GetEnumElement(model.Code));
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
                .Include(e => e.Description).ThenInclude(e => e.Tags)
                .Include(e=> e.AssignedTo)
                .Include(e => e.Parent)
                .Include(e=>e.Epic)
                .Include(e=>e.Children);

            var entity = await query
                .FirstOrDefaultAsync(e => e.Description.ProjectId == model.ProjectId && e.Number == number && e.Type == type);
           
            if (entity == null)
                throw new NotFoundException("item_nf");

            nextentity.DescriptionId = entity.DescriptionId;
            nextentity.Type = type;
            nextentity.Number = entity.Number;
            nextentity.Status = entity.Status;
            WorkItemHelper.RestoreDescriptionData(entity.Description, nextdesc);

            if (model.ParentId != null)
            {
                operRes.Plus(await CheckParent<UserStory>(model.ProjectId, model.ParentId.Value));
                entity.EpicId = null;
            }

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
                Name = e.AssignedTo.FullName,
                Id = e.AssignedToId.Value,
                UserName = e.AssignedTo.UserName
            },
            Code = e.Type.Code(),
            DescriptionId = e.DescriptionId,         
            Name = e.Name,
            Number = e.Number,
            Status = e.Status,
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
                CreatedBy = e.Description.Creator == null ? null : new UserLightDTO
                {
                    Avatar = e.Description.Creator.Avatar,
                    Id = e.Description.CreatorId,
                    Name = e.Description.Creator.FullName
                },
                DateOfCreation = e.Description.DateOfCreation,
                DescriptionText = e.Description.DescriptionText,
                LastUpdate = e.Description.LastUpdate,
                LastUpdateBy = e.Description.LastUpdater == null ? null : new UserLightDTO
                {
                    Avatar = e.Description.LastUpdater.Avatar,
                    Id = e.Description.LastUpdaterId.Value,
                    Name = e.Description.LastUpdater.FullName
                }
            },
            Epic = e.Epic!=null?new ItemDTO
            {
                Code = WorkItemType.Epic.Code(),
                Name = e.Epic.Name,
                Number = e.Epic.Number,
                DescriptionId = e.Epic.DescriptionId
            }:(e.Parent!=null?(e.Parent.Parent!=null? new ItemDTO
            {
                Code = WorkItemType.Epic.Code(),
                Name = e.Parent.Parent.Name,
                Number = e.Parent.Parent.Number,
                DescriptionId = e.Parent.Parent.DescriptionId
            } : null):null),
            SprintId = e.SprintId,
            SprintNumber = e.Sprint == null?null:(int?)e.Sprint.Number
        };
    }
}
