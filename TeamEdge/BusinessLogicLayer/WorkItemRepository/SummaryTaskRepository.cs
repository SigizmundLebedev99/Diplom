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
    class SummaryTaskRepository : WorkItemRepository
    {
        public SummaryTaskRepository(IServiceProvider provider) : base(provider)
        {
        }

        public override async Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model, UserProject userProj = null)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            //var entity = _mapper.Map<SummaryTask>(model);

            ////var checkResult = await CheckChildren(model.ChildrenIds, model.ProjectId, 
            ////    _context.Tasks.Concat((IQueryable<IBaseWorkItemWithParent<SummaryTask>>)_context.SummaryTasks));
            ////operRes.Plus(checkResult);

            //if (model.ParentId != null)
            //    operRes.Plus(await CheckParent<SummaryTask>(model.ProjectId, model.ParentId.Value));

            //if (!operRes.Succeded)
            //    return operRes;

            ////var children = checkResult.Result;
            //entity.Number = await GetNumber<SummaryTask>(model.ProjectId);
            //entity.DescriptionId = description.Id;

            //AddChildren(checkResult.Result, 
            //    description.Id);

            //_context.SummaryTasks.Add(entity);

            //await _context.SaveChangesAsync();
            //operRes.Result = await _context.SummaryTasks.Select(SelectExpression).FirstOrDefaultAsync(e => e.DescriptionId == description.Id);
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

        public override async Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int number, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);

            //var nextentity = _mapper.Map<SummaryTask>(model);
            //var nextdesc = _mapper.Map<WorkItemDescription>(model);

            //var query = _context.SummaryTasks
            //    .Include(e => e.Description).ThenInclude(e => e.Files).ThenInclude(e => e.File)
            //    .Include(e=> e.Description).ThenInclude(e=>e.Tags)
            //    .Include(e => e.Children)
            //    .Include(e=>e.SummaryTaskChildren)
            //    .Include(e => e.Parent);

            //var entity = await query
            //    .FirstOrDefaultAsync(e => e.Description.ProjectId == model.ProjectId && e.Number == number);

            //if (entity == null)
            //    throw new NotFoundException("item_nf");

            //nextentity.DescriptionId = entity.DescriptionId;
            //nextentity.Number = entity.Number;
            //WorkItemHelper.RestoreDescriptionData(entity.Description, nextdesc);

            //var checkResult = await CheckChildren(model.ChildrenIds, model.ProjectId,
            //   _context.Tasks.Concat((IQueryable<IBaseWorkItemWithParent<SummaryTask>>)_context.SummaryTasks));
            //operRes.Plus(checkResult);
            //operRes.Plus(CheckStatus(checkResult.Result, entity.Status));

            //if (model.ParentId != null)
            //    operRes.Plus(await CheckParent<Epick>(model.ProjectId, model.ParentId.Value));

            //if (!operRes.Succeded)
            //    return operRes;

            //var files = nextdesc.Files;
            //var tags = nextdesc.Tags;
            //nextdesc.Files = null;
            //nextdesc.Tags = null;
            //DetachAllEntities(entity);
            //_context.WorkItemDescriptions.Update(nextdesc);
            //UpdateFiles(entity.Description.Files, files, nextdesc.Id);
            //UpdateTags(entity.Description.Tags, tags);
            //UpdateChildren(entity.AllChildren, checkResult.Result, entity.DescriptionId);
            //_context.SummaryTasks.Update(nextentity);

            //await _context.SaveChangesAsync();

            //var result = await query.Where(e => e.DescriptionId == entity.DescriptionId).ToListAsync();
            //operRes.Result = result.Select(SelectExpression.Compile()).First();
            //_historyService.CompareForChanges(entity, result.First(), _httpContext.User);
            return operRes;
        }

        public override Task<ItemDTO> GetDenseWorkItem(string code, int number, int projectId)
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
