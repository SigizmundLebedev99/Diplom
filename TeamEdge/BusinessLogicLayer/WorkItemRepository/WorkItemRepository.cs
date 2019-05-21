using AutoMapper;
using Microsoft.AspNetCore.Http;
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
    abstract class WorkItemRepository
    {
        protected readonly TeamEdgeDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly IHistoryService _historyService;
        protected readonly HttpContext _httpContext;

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

        protected void DetachAllEntities(object entity)
        {
            var t = entity.GetType();
            _context.Entry(entity).State = EntityState.Detached;
            foreach(var prop in t.GetProperties())
            {
                var value = prop.GetValue(entity);
                if (value == null)
                    continue;
                switch (value)
                {
                    case BaseEntity ent:
                        {
                            _context.Entry(ent).State = EntityState.Detached;
                            DetachAllEntities(ent);
                            break;
                        }
                    case IEnumerable<object> en:
                        {
                            foreach(var obj in en)
                            {
                                _context.Entry(obj).State = EntityState.Detached;
                            }
                            break;
                        }
                    case ValueType val:
                    case string srt:
                        { break; }
                }
            }
        }

        protected void AddChildren<TChild>(IEnumerable<TChild> children, int parentId) where TChild : IBaseWorkItemWithParent<TChild>
        {
            if (children != null)
            {
                foreach (var t in children)
                    t.ParentId = parentId;
                _context.UpdateRange(children);
            }
        }

        protected void UpdateChildren<TChild, TParent>(IEnumerable<TChild> previous, IEnumerable<TChild> next, int parentId)
            where TChild:class, IBaseWorkItemWithParent<TParent>
            where TParent:class, IBaseWorkItemWithChild<TChild>
        {
            IEnumerable<TChild> resultSeq;

            if ((next == null || next.Count() == 0) && (previous == null || previous.Count() == 0))
                return;
            else if (previous == null || previous.Count() == 0)
            {
                foreach (var ch in next)
                {
                    ch.ParentId = parentId;
                    ch.Parent = null;
                }
                resultSeq = next;
            }

            else if (next == null || next.Count() == 0)
            {
                foreach (var ch in previous)
                {
                    ch.ParentId = null;
                    ch.Parent = null;
                }
                resultSeq = previous;
            }

            else
            {
                var deleted = previous.Except(next, new WorkItemComparer<TChild>());
                foreach (var ch in deleted)
                {
                    ch.ParentId = null;
                    ch.Parent = null;
                }

                var added = next.Except(previous, new WorkItemComparer<TChild>());
                foreach (var ch in added)
                {
                    ch.ParentId = parentId;
                    ch.Parent = null;
                }
                resultSeq = added.Concat(deleted);
            } 
            
            _context.Set<TChild>().UpdateRange(resultSeq);
        }

        protected void UpdateFiles(IEnumerable<WorkItemFile> previous, IEnumerable<WorkItemFile> next, int itemId)
        {
            if ((next == null || next.Count() == 0) && (previous == null || previous.Count() == 0))
                return;

            else if (previous == null || previous.Count() == 0)
            {
                foreach(var n in next)
                {
                    n.WorkItem = null;
                    n.WorkItemId = itemId;
                }
                _context.WorkItemFiles.AddRange(next);
            }

            else if (next == null || next.Count() == 0)
            {
                foreach(var p in previous)
                {
                    p.WorkItem = null;
                }
                _context.WorkItemFiles.RemoveRange(previous);
            }

            else
            {
                previous = previous.Select(e => { e.WorkItem = null; return e; });
                next = next.Select(e => { e.WorkItem = null; e.WorkItemId = itemId ; return e; });
                var deleted = previous.Except(next, new FileComparer());
                _context.WorkItemFiles.RemoveRange(deleted);
                var added = next.Except(previous, new FileComparer());
                _context.WorkItemFiles.AddRange(added);
            }
        }

        protected void UpdateTags(IEnumerable<WorkItemTag> previous, IEnumerable<WorkItemTag> next)
        {
            if ((next == null || next.Count() == 0) && (previous == null || previous.Count() == 0))
                return;

            else if (previous == null || previous.Count() == 0)
            {
                foreach (var n in next)
                {
                    n.WorkItem = null;
                }
                _context.WorkItemTags.AddRange(next);
            }

            else if (next == null || next.Count() == 0)
            {
                foreach (var p in previous)
                {
                    p.WorkItem = null;
                }
                _context.WorkItemTags.RemoveRange(previous);
            }

            else
            {
                previous = previous.Select(e => { e.WorkItem = null; return e; });
                next = next.Select(e => { e.WorkItem = null; return e; });
                var deleted = previous.Except(next, new TagComparer());
                _context.WorkItemTags.RemoveRange(deleted);
                var added = next.Except(previous, new TagComparer());
                _context.WorkItemTags.AddRange(added);
            }
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

        protected Task<OperationResult<IEnumerable<T>>> CheckChildren<T>(int[] childrenIds, int projectId) where T : BaseWorkItem
        {
            return CheckChildrenTemplate(childrenIds, projectId, _context.Set<T>());
        }

        protected async Task<OperationResult<IEnumerable<T>>> CheckChildren<T>(int[] childrenIds, int projectId, IQueryable<T> context) where T : IBaseWorkItem
        {
            return await CheckChildrenTemplate(childrenIds, projectId, context);
        }

        protected async Task<OperationResult<IEnumerable<T>>> CheckChildrenTemplate<T>(int[] childrenIds, int projectId, IQueryable<T> context) where T : IBaseWorkItem
        {
            var operRes = new OperationResult<IEnumerable<T>>(true);
            T[] children = null;
            if (childrenIds != null && childrenIds.Length > 0)
            {
                children = await context
                    .Where(t => t.Description.ProjectId == projectId
                    && childrenIds.Contains(t.DescriptionId)).ToArrayAsync();

                if (children.Length < childrenIds.Length)
                {
                    foreach (var i in childrenIds.Where(i => !children.Any(e => e.DescriptionId == i)))
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
            _httpContext = ((IHttpContextAccessor)provider.GetService(typeof(IHttpContextAccessor))).HttpContext;
        }

        public abstract Task<WorkItemDTO> GetWorkItem(string code, int number, int projectId);
        public abstract Task<ItemDTO> GetDenseWorkItem(string code, int number, int projectId);
        public abstract Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model, UserProject userProj = null);
        public abstract IQueryable<ItemDTO> GetItems(GetItemsDTO model);
        public abstract Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int number, CreateWorkItemDTO model);
    }

    class FileComparer : IEqualityComparer<WorkItemFile>
    {
        public bool Equals(WorkItemFile x, WorkItemFile y)
        {
            return x.FileId == y.FileId;
        }

        public int GetHashCode(WorkItemFile obj)
        {
            return (obj.FileId + obj.WorkItemId).GetHashCode();
        }
    }

    class TagComparer : IEqualityComparer<WorkItemTag>
    {
        public bool Equals(WorkItemTag x, WorkItemTag y)
        {
            return x.Tag == y.Tag;
        }

        public int GetHashCode(WorkItemTag obj)
        {
            return obj.Tag.GetHashCode();
        }
    }
}
