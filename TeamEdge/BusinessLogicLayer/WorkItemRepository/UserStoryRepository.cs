using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class UserStoryRepository : WorkItemRepository
    {
        public UserStoryRepository(TeamEdgeDbContext context, IMapper mapper) : base(context, mapper) { }

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

        private static readonly Expression<Func<UserStory, WorkItemDTO>> SelectExpression = e => new UserStoryInfoDTO
        {
            Code = WorkItemType.UserStory.Code(),
            DescriptionId = e.DescriptionId,
            Name = e.Name,
            Number = e.Number,
            AcceptenceCriteria = e.AcceptenceCriteria,
            AcceptenceCriteriaCode = e.AcceptenceCriteriaCode,
            Risk = e.Risk,
            Priority = e.Priority,
            SprintId = e.SprintId,
            SprintName = e.SprintId == null? null : e.Sprint.Name,
            Status = e.Status,
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
                DescriptionCode = e.Description.DescriptionCode,
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
