using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public abstract class WorkItemRepository
    {
        protected readonly TeamEdgeDbContext _context;
        protected readonly IMapper _mapper;

        protected async Task<int> GetNumber<T>(int projId, Expression<Func<T, bool>> extraFilter = null) where T : BaseWorkItem
        {
            IQueryable<T> set = null;
            if (extraFilter != null)
                set = _context.Set<T>().Where(e => e.Description.ProjectId == projId).Where(extraFilter);
            else
                set = _context.Set<T>().Where(e => e.Description.ProjectId == projId);

            if (await set.AnyAsync())
            {
                var number = await set
                    .Select(e => e.Number)
                    .MaxAsync();
                return number + 1;
            }
            else
                return 1;
        }

        protected async Task<OperationResult> CheckParent<T>(int projectId, int parentId) where T : BaseWorkItem
        {
            var operRes = new OperationResult(true);

            if (!await _context.Set<T>()
                .AnyAsync(e => e.Description.ProjectId == projectId
                && e.DescriptionId == parentId))
                operRes.AddErrorMessage("parent_nf", parentId);

            return operRes;
        }

        protected async Task<OperationResult<IEnumerable<T>>> CheckChildren<T>(int[] childrenIds, int projectId) where T : BaseWorkItem
        {
            var operRes = new OperationResult<IEnumerable<T>>(true);
            T[] children = null;
            if (childrenIds != null && childrenIds.Length > 0)
            {
                children = await _context.Set<T>()
                    .Where(t => t.Description.ProjectId == projectId
                    && childrenIds.Contains(t.DescriptionId)).ToArrayAsync();

                if (children.Length < childrenIds.Length)
                {
                    foreach (var i in childrenIds.Where(i => !children.Any(e=>e.DescriptionId == i)))
                    {
                        operRes.AddErrorMessage("children_nf", i);
                    }
                }
                else
                {
                    operRes.Result = children;
                }
            }
            return operRes;
        }  

        public WorkItemRepository(TeamEdgeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public abstract Task<WorkItemDTO> GetWorkItem(string code, int number, int project);
        public abstract Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model);
        public abstract IQueryable<ItemDTO> GetItems(GetItemsDTO model);
    }
}
