using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.BusinessLogicLayer.Services;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class EpickRepository : WorkItemRepository
    {
        public EpickRepository(TeamEdgeDbContext context, IMapper mapper) : base(context, mapper) { }

        public override Task<WorkItemDTO> GetWorkItem(string code, int number, int project)
        {
            return _context.Epicks.Where(e => e.Description.ProjectId == project && e.Number == number)
                .Select(SelectExpression).FirstOrDefaultAsync();
        }

        public override async Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            var entity = _mapper.Map<Epick>(model);

            var checkResult = await CheckChildren<Feature>(model.ChildrenIds, model.ProjectId);
            operRes.Plus(checkResult);

            if (!operRes.Succeded)
                return operRes;

            var children = checkResult.Result;
            entity.Number = await GetNumber<Epick>(model.ProjectId);
            entity.DescriptionId = description.Id;

            if (children != null)
            {
                foreach (var t in children)
                    t.ParentId = entity.DescriptionId;
                _context.Features.UpdateRange(children);
            }

            _context.Epicks.Add(entity);

            await _context.SaveChangesAsync();
            operRes.Result = await _context.Epicks.Select(SelectExpression).FirstOrDefaultAsync(e => e.DescriptionId == description.Id);
            return operRes;
        }

        private static readonly Expression<Func<Epick, WorkItemDTO>> SelectExpression = e => new WorkItemDTO
        {
            Code = WorkItemType.Epick.Code(),
            DescriptionId = e.DescriptionId,
            Name = e.Name,
            Number = e.Number,
            Status = e.Status,
            Children = e.Children.Select(a => new ItemDTO
            {
                Code = WorkItemType.Feature.Code(),
                Name = a.Name,
                Number = a.Number,
                DescriptionId = a.DescriptionId
            }),
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
            }
        };
    }
}
