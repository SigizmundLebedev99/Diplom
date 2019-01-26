using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public abstract class WorkItemRepository
    {
        protected readonly TeamEdgeDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly IHistoryService _historyService;

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

        protected void AddChildren<TChild, TPar>(IEnumerable<TChild> children, int parentId)
            where TChild : BaseWorkItem, IBaseWorkItemWithParent<TPar>
            where TPar : BaseWorkItem
        {
            if (children != null)
            {
                foreach (var t in children)
                    t.ParentId = parentId;
                _context.Set<TChild>().UpdateRange(children);
            }
        }

        protected void UpdateChildren<TChild, TPar>(IEnumerable<TChild> previous, IEnumerable<TChild> next, int parentId) 
            where TChild : BaseWorkItem, IBaseWorkItemWithParent<TPar> 
            where TPar: BaseWorkItem
        {
            IEnumerable<TChild> resultSeq;
            if ((next == null || next.Count() == 0) && (previous == null || previous.Count() == 0))
                return;

            else if (previous == null || previous.Count() == 0)
            {
                foreach (var ch in next)
                    ch.ParentId = parentId;
                resultSeq = next;
            }

            else if (next == null || next.Count() == 0)
            {
                foreach (var ch in previous)
                    ch.ParentId = null;
                resultSeq = previous;
            }

            else
            {
                var deleted = previous.Except(next, new WorkItemComparer<TChild>());
                foreach (var ch in deleted)
                    ch.ParentId = null;

                var added = next.Except(previous, new WorkItemComparer<TChild>());
                foreach (var ch in added)
                    ch.ParentId = parentId;

                resultSeq = added.Concat(deleted);
            } 
            
            _context.Set<TChild>().UpdateRange(resultSeq);
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

        public WorkItemRepository(IServiceProvider provider)
        {
            _context = (TeamEdgeDbContext)provider.GetService(typeof(TeamEdgeDbContext));
            _mapper = (IMapper)provider.GetService(typeof(IMapper));
            _historyService = (IHistoryService)provider.GetService(typeof(IHistoryService));
        }

        public abstract Task<WorkItemDTO> GetWorkItem(string code, int number, int project);
        public abstract Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model);
        public abstract IQueryable<ItemDTO> GetItems(GetItemsDTO model);
        public abstract Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int number, CreateWorkItemDTO model);
    }

    class WorkItemComparer<T> : IEqualityComparer<T> where T : BaseWorkItem
    {
        public bool Equals(T x, T y)
        {
            return x.DescriptionId == y.DescriptionId;
        }

        public int GetHashCode(T obj)
        {
            return obj.DescriptionId.GetHashCode();
        }
    }
}
