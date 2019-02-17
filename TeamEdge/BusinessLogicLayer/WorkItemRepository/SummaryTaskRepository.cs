using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Helpers;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class SummaryTaskRepository : WorkItemRepository
    {
        public SummaryTaskRepository(IServiceProvider provider) : base(provider)
        {
        }

        public override async Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            var entity = _mapper.Map<SummaryTask>(model);

            var checkResult = await CheckChildren(model.ChildrenIds, model.ProjectId, 
                _context.Tasks.Concat((IQueryable<BaseWorkItem>)_context.SummaryTasks));
            operRes.Plus(checkResult);

            if (model.ParentId != null)
                operRes.Plus(await CheckParent<SummaryTask>(model.ProjectId, model.ParentId.Value));

            if (!operRes.Succeded)
                return operRes;

            var children = checkResult.Result;
            entity.Number = await GetNumber<SummaryTask>(model.ProjectId);
            entity.DescriptionId = description.Id;

            AddChildren<IBaseWorkItemWithParent<SubTask>, SubTask>(checkResult.Result
                .Select(e=>(IBaseWorkItemWithParent<SubTask>)e), 
                description.Id);

            _context.SummaryTasks.Add(entity);

            await _context.SaveChangesAsync();
            operRes.Result = await _context.SummaryTasks.Select(SelectExpression).FirstOrDefaultAsync(e => e.DescriptionId == description.Id);
            return operRes;
        }

        public override IQueryable<ItemDTO> GetItems(GetItemsDTO model)
        {
            var filter = WorkItemHelper.GetFilter<SummaryTask>(model);
            return _context.SummaryTasks.Where(filter).Select(WorkItemHelper.ItemDTOSelector);
        }

        public override Task<WorkItemDTO> GetWorkItem(string code, int number, int project)
        {
            return _context.SummaryTasks.Where(e => e.Description.ProjectId == project && e.Number == number)
                .Select(SelectExpression)
                .FirstOrDefaultAsync();
        }

        public override Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int number, CreateWorkItemDTO model)
        {
            throw new NotImplementedException();
        }

        private Expression<Func<SummaryTask, WorkItemDTO>> SelectExpression = e => new SummaryTaskInfoDTO
        {
            Code = e.Code,
            Status = e.Status.ToString(),
            StartDate = e.StartDate,
            EndDate = e.EndDate,
            Duration = e.Duration.ToTimeSpan(),
            DescriptionId = e.DescriptionId,
            Name = e.Name,
            Number = e.Number,
            Description = new DescriptionDTO
            {
                CreatedBy = new UserLightDTO
                {
                    Avatar = e.Description.Creator.Avatar,
                    Id = e.Description.CreatorId,
                    Name = e.Description.Creator.FullName
                },
                DateOfCreation = e.Description.DateOfCreation,
                Description = e.Description.DescriptionText,
                FilesCount = e.Description.Files.Count(),
                LastUpdate = e.Description.LastUpdate,
                LastUpdateBy = e.Description.LastUpdater == null ? null : new UserLightDTO
                {
                    Avatar = e.Description.LastUpdater.Avatar,
                    Id = e.Description.LastUpdaterId.Value,
                    Name = e.Description.LastUpdater.FullName
                }
            },
            Children = e.AllChildren.Select(a =>
                new ItemDTO
                {
                    Code = a.Code,
                    DescriptionId = a.DescriptionId,
                    Name = a.Name,
                    Number = a.Number,
                    Status = a.Status
                }),
            Parent = e.Parent == null? null:new ItemDTO
            {
                Code = e.Parent.Code,
                Status = e.Parent.Status,
                Name = e.Parent.Name,
                DescriptionId = e.ParentId.Value,
                Number = e.Parent.Number
            }
        };

    }
}
