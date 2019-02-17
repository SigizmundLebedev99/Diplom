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
    public class UserStoryRepository : WorkItemRepository
    {
        public UserStoryRepository(IServiceProvider provider) : base(provider) { }

        public override Task<WorkItemDTO> GetWorkItem(string code, int number, int project)
        {
            return _context.UserStories.Where(e => e.Description.ProjectId == project && e.Number == number)
                .Select(SelectExpression).FirstOrDefaultAsync();
        }

        public override async Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            var entity = _mapper.Map<UserStory>(model);

            var checkResult = await CheckChildren<_Task>(model.ChildrenIds, model.ProjectId);
            operRes.Plus(checkResult);

            if (model.ParentId != null)
                operRes.Plus(await CheckParent<Feature>(model.ProjectId, model.ParentId.Value));
            
            if (!operRes.Succeded)
                return operRes;

            var children = checkResult.Result;
            entity.Number = await GetNumber<UserStory>(model.ProjectId);
            entity.DescriptionId = description.Id;

            if (children != null)
            {
                foreach (var t in children)
                    t.ParentId = entity.DescriptionId;
                _context.Tasks.UpdateRange(children);
            }
            
            _context.UserStories.Add(entity);

            await _context.SaveChangesAsync();
            operRes.Result = await _context.UserStories.Select(SelectExpression).FirstOrDefaultAsync(e => e.DescriptionId == description.Id);
            return operRes;
        }

        public override IQueryable<ItemDTO> GetItems(GetItemsDTO model)
        {
            var filter = WorkItemHelper.GetFilter<UserStory>(model);
            return _context.UserStories.Where(filter).Select(WorkItemHelper.ItemDTOSelector);
        }

        public override async Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int number, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);

            var nextentity = _mapper.Map<UserStory>(model);
            var nextdesc = _mapper.Map<WorkItemDescription>(model);

            var query = _context.UserStories
                .Include(e => e.Description).ThenInclude(e => e.Files).ThenInclude(e => e.File)
                .Include(e => e.Description).ThenInclude(e => e.Branches)
                .Include(e => e.Children);

            var entity = await query
                .FirstOrDefaultAsync(e => e.Description.ProjectId == model.ProjectId && e.Number == number);

            if (entity == null)
                throw new NotFoundException("item_nf");

            nextentity.DescriptionId = entity.DescriptionId;
            nextentity.Number = entity.Number;
            WorkItemHelper.RestoreDescriptionData(entity.Description, nextdesc);

            var checkResult = await CheckChildren<_Task>(model.ChildrenIds, model.ProjectId);
            if (model.ParentId != null)
                operRes.Plus(await CheckParent<Feature>(model.ProjectId, model.ParentId.Value));

            operRes.Plus(checkResult);
            operRes.Plus(CheckStatus(checkResult.Result, entity.Status));

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
            UpdateChildren<_Task, UserStory>(entity.Children, checkResult.Result, entity.DescriptionId);
            _context.UserStories.Update(nextentity);

            await _context.SaveChangesAsync();

            var result = await query.Where(e => e.DescriptionId == entity.DescriptionId).ToListAsync();
            operRes.Result = result.Select(SelectExpression.Compile()).First();
            _historyService.CompareForChanges(entity, result.First(), _httpContext.User);
            return operRes;
        }

        private static readonly Expression<Func<UserStory, WorkItemDTO>> SelectExpression = e => new UserStoryInfoDTO
        {
            Code = WorkItemType.UserStory.Code(),
            DescriptionId = e.DescriptionId,
            Name = e.Name,
            Number = e.Number,
            AcceptenceCriteria = e.AcceptenceCriteria,
            Priority = e.Priority,
            SprintId = e.SprintId,
            SprintName = e.SprintId == null? null : e.Sprint.Name,
            Status = e.Status.ToString(),
            Children = e.Children.Select(a=>new ItemDTO
            {
                DescriptionId = a.DescriptionId,
                Code = a.Type.Code(),
                Name = a.Name,
                Number = a.Number
            }),
            Parent = e.Parent == null? null : new ItemDTO
            {
                DescriptionId = e.Parent.DescriptionId,
                Code = WorkItemType.Feature.Code(),
                Name = e.Parent.Name,
                Number = e.Parent.Number
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
