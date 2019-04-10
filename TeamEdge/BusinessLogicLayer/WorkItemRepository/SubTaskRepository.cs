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
    class SubTaskRepository : WorkItemRepository
    {
        public SubTaskRepository(IServiceProvider provider) : base(provider) { }

        public override async Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model, UserProject userProj = null)
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
                .Include(e => e.Description).ThenInclude(e => e.Tags)
                .Include(e => e.Parent);

            var entity = await query
                .FirstOrDefaultAsync(e => e.Description.ProjectId == model.ProjectId && e.Number == number);

            if (entity == null)
                throw new NotFoundException("item_nf");

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

        public override Task<ItemDTO> GetDenseWorkItem(string code, int number, int projectId)
        {
            return _context.SubTasks.Where(e => e.Description.ProjectId == projectId && e.Number == number)
               .Select(WorkItemHelper.ItemDTOSelector).FirstOrDefaultAsync();
        }

        private static readonly Expression<Func<SubTask, WorkItemDTO>> SelectExpression = e => new TaskInfoDTO
        {
            Code = WorkItemType.SubTask.Code(),
            DescriptionId = e.DescriptionId,
            Name = e.Name,
            Number = e.Number,
            Status = e.Status.ToString(),
            Parent = e.Parent == null ? null : new ItemDTO
            {
                Code = e.Parent.Code,
                Name = e.Parent.Name,
                Number = e.Parent.Number,
                DescriptionId = e.Parent.DescriptionId
            },
            Description = new DescriptionDTO
            {
                CreatedBy = e.Description.Creator == null? null:new UserLightDTO
                {
                    Avatar = e.Description.Creator.Avatar,
                    Id = e.Description.CreatorId,
                    Name = e.Description.Creator.FullName
                },
                DateOfCreation = e.Description.DateOfCreation,
                Description = e.Description.DescriptionText,
                LastUpdate = e.Description.LastUpdate,
                LastUpdateBy = e.Description.LastUpdaterId == null ? null : new UserLightDTO
                {
                    Avatar = e.Description.Creator.Avatar,
                    Id = e.Description.CreatorId,
                    Name = e.Description.Creator.FullName
                }
            }
        };
    }
}
