﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
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
    public class EpickRepository : WorkItemRepository
    {
        public EpickRepository(IServiceProvider provider) : base(provider) { }

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

        public override IQueryable<ItemDTO> GetItems(GetItemsDTO model)
        {
            model.HasNoParent = false;
            model.ParentId = null;
            var filter = WorkItemHelper.GetFilter<Epick>(model);
            return _context.Epicks.Where(filter).Select(WorkItemHelper.ItemDTOSelector);
        }

        public override async Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int number, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);

            var nextentity = _mapper.Map<Epick>(model);
            var nextdesc = _mapper.Map<WorkItemDescription>(model);

            var query = _context.Epicks
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

            var checkResult = await CheckChildren<Feature>(model.ChildrenIds, model.ProjectId);

            operRes.Plus(checkResult);
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
            UpdateChildren<Feature, Epick>(entity.Children, checkResult.Result, entity.DescriptionId);
            _context.Epicks.Update(nextentity);

            await _context.SaveChangesAsync();

            var result = await query.Where(e => e.DescriptionId == entity.DescriptionId).ToListAsync();
            operRes.Result = result.Select(SelectExpression.Compile()).First();
            _historyService.CompareForChanges(entity, result.First(), _httpContext.User);
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
