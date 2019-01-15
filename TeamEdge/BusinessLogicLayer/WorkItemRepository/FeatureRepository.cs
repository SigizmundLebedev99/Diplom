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
    public class FeatureRepository : WorkItemRepository
    {
        public FeatureRepository(TeamEdgeDbContext context, IMapper mapper) : base(context, mapper) { }

        public override Task<WorkItemDTO> GetWorkItem(string code, int number, int project)
        {
            return _context.Features.Where(e => e.Description.ProjectId == project && e.Number == number)
                .Select(SelectExpression)
                .FirstOrDefaultAsync();
        }

        public override async Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            var entity = _mapper.Map<Feature>(model);

            var checkResult = await CheckChildren<UserStory>(model.ChildrenIds, model.ProjectId);
            operRes.Plus(checkResult);

            if (model.ParentId != null)
                operRes.Plus(await CheckParent<Epick>(model.ProjectId, model.ParentId.Value));

            if (!operRes.Succeded)
                return operRes;

            var children = checkResult.Result;
            entity.Number = await GetNumber<Feature>(model.ProjectId);
            entity.DescriptionId = description.Id;

            if (children != null)
            {
                foreach (var t in children)
                    t.ParentId = entity.DescriptionId;
                _context.UserStories.UpdateRange(children);
            }

            _context.Features.Add(entity);

            await _context.SaveChangesAsync();
            operRes.Result = await _context.Features.Select(SelectExpression).FirstOrDefaultAsync(e => e.DescriptionId == description.Id);
            return operRes;
        }

        private static Expression<Func<Feature, WorkItemDTO>> SelectExpression = e => new WorkItemDTO
        {
            Code = WorkItemType.Feature.Code(),
            Name = e.Name,
            Number = e.Number,
            Status = e.Status,
            Children = e.Children.Select(a => new ItemDTO
            {
                Code = WorkItemType.UserStory.Code(),
                Name = a.Name,
                Number = a.Number,
                DescriptionId = a.DescriptionId
            }),
            Parent = e.Parent == null ? null : new ItemDTO
            {
                Code = WorkItemType.Epick.Code(),
                Name = e.Parent.Name,
                Number = e.Parent.Number,
                DescriptionId = e.Parent.DescriptionId
            },
            DescriptionId = e.DescriptionId,
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
                },
                FilesCount = e.Description.Files.Count()
            },
        };
    }
}
