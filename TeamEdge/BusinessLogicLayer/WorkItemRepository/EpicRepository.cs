using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Services;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    class EpicRepository : WorkItemRepository
    {
        public EpicRepository(IServiceProvider provider) : base(provider) { }

        public override Task<WorkItemDTO> GetWorkItem(string code, int number, int project)
        {
            return _context.Epics.Where(e => e.Description.ProjectId == project && e.Number == number)
                .Select(SelectExpression).FirstOrDefaultAsync();
        }

        public override async Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model, UserProject userProj = null)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            var entity = _mapper.Map<Epic>(model);

            var epickDto = model as CreateEpickDTO;
            var checkLinksResult = await CheckChildren<_Task>(epickDto.LinkIds, model.ProjectId);
            operRes.Plus(checkLinksResult);
            var checkResult = await CheckChildren<UserStory>(model.ChildrenIds, model.ProjectId);
            operRes.Plus(checkResult);

            if (!operRes.Succeded)
                return operRes;
            var children = checkResult.Result;
            entity.Number = await GetNumber<Epic>(model.ProjectId);
            entity.DescriptionId = description.Id;

            if (children != null)
            {
                foreach (var t in children)
                    t.ParentId = description.Id;
                _context.UserStories.UpdateRange(children);
            }
            if (checkLinksResult.Result != null)
            {
                foreach (var t in checkLinksResult.Result)
                    t.EpicId = description.Id;
                _context.Tasks.UpdateRange(checkLinksResult.Result);
            }

            _context.Epics.Add(entity);

            await _context.SaveChangesAsync();
            operRes.Result = await _context.Epics.Select(SelectExpression).FirstOrDefaultAsync(e => e.DescriptionId == description.Id);
            return operRes;
        }

        public override IQueryable<ItemDTO> GetItems(GetItemsDTO model)
        {
            if (model.Code.EndsWith('!'))
            {
                return _context.UserStories.Select(WorkItemHelper.ItemDTOSelector).Concat(
                    _context.Tasks.Where(e=>e.ParentId == null).Select(WorkItemHelper.ItemDTOSelector));
            }
            model.HasNoParent = false;
            model.ParentId = null;
            var filter = WorkItemHelper.GetFilter<Epic>(model);
            return _context.Epics.Where(filter).Select(WorkItemHelper.ItemDTOSelector);
        }

        public override async Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int number, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            var newModel = model as CreateEpickDTO;
            var nextentity = _mapper.Map<Epic>(model);
            var nextdesc = _mapper.Map<WorkItemDescription>(model);

            var query = _context.Epics
                .Include(e => e.Description).ThenInclude(e => e.Files).ThenInclude(e => e.File)
                .Include(e=>e.Description).ThenInclude(e=>e.Tags)
                .Include(e=>e.Links)
                .Include(e => e.Children);

            var entity = await query
                .FirstOrDefaultAsync(e => e.Description.ProjectId == model.ProjectId && e.Number == number);

            if (entity == null)
                throw new NotFoundException("item_nf");

            nextentity.DescriptionId = entity.DescriptionId;
            nextentity.Number = entity.Number;
            WorkItemHelper.RestoreDescriptionData(entity.Description, nextdesc);

            var checkResult = await CheckChildren<UserStory>(model.ChildrenIds, model.ProjectId);
            var checkLinks = await CheckChildren<_Task>(newModel.LinkIds, model.ProjectId);
            operRes.Plus(checkResult);
            operRes.Plus(checkLinks);
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
            UpdateChildren<UserStory, Epic>(entity.Children, checkResult.Result, entity.DescriptionId);
            UpdateChildren<_Task, Epic>(entity.Links, checkLinks.Result, entity.DescriptionId);
            _context.Epics.Update(nextentity);

            await _context.SaveChangesAsync();

            var result = await query.Where(e => e.DescriptionId == entity.DescriptionId).ToListAsync();
            operRes.Result = result.Select(SelectExpression.Compile()).First();
            _historyService.CompareForChanges(entity, result.First(), _httpContext.User);
            return operRes;
        }

        public override Task<ItemDTO> GetDenseWorkItem(string code, int number, int projectId)
        {
            return _context.Epics.Where(e => e.Description.ProjectId == projectId && e.Number == number)
                .Select(WorkItemHelper.ItemDTOSelector)
                .FirstOrDefaultAsync();
        }

        private static readonly Expression<Func<Epic, WorkItemDTO>> SelectExpression = e => new WorkItemDTO
        {
            Code = WorkItemType.Epic.Code(),
            DescriptionId = e.DescriptionId,
            Name = e.Name,
            Number = e.Number,
            Children = e.Children.Select(a => new ItemDTO
            {
                Code = WorkItemType.UserStory.Code(),
                Name = a.Name,
                Number = a.Number,
                DescriptionId = a.DescriptionId
            }).Concat(e.Links.Where(a=>a.ParentId == null).Select(a => new ItemDTO
            {
                Code = a.Code,
                Name = a.Name,
                Number = a.Number,
                DescriptionId = a.DescriptionId
            })),
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
                    Avatar = e.Description.Creator.Avatar,
                    Id = e.Description.CreatorId,
                    Name = e.Description.Creator.FullName
                },
                FilesCount = e.Description.Files.Count()
            }
        };
    }
}
