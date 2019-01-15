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
    public class TaskRepository : WorkItemRepository
    {
        public TaskRepository(TeamEdgeDbContext context, IMapper mapper) : base(context, mapper) { }

        public override Task<WorkItemDTO> GetWorkItem(string code, int number, int project)
        {
            TaskType type = Enum.Parse<TaskType>(WorkItemFactory.GetEnumElement(code));
            return _context.Tasks
                .Where(e => e.Type == type && e.Number == number && e.Description.ProjectId == project)
                .Select(SelectExpression)
                .FirstOrDefaultAsync();
        }

        public override async Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            var entity = _mapper.Map<_Task>(model);

            var checkResult = await CheckChildren<_Task>(model.ChildrenIds, model.ProjectId);
            operRes.Plus(checkResult);

            if (model.ParentId != null)
                operRes.Plus(await CheckParent<UserStory>(model.ProjectId, model.ParentId.Value));

            if (!operRes.Succeded)
                return operRes;
            
            var children = checkResult.Result;

            entity.Type = Enum.Parse<TaskType>(WorkItemFactory.GetEnumElement(model.Code));
            entity.Number = await GetNumber<_Task>(model.ProjectId, t=>t.Type == entity.Type);
            entity.DescriptionId = description.Id;

            if (children != null)
            {
                foreach (var t in children)
                    t.ParentId = entity.DescriptionId;
                _context.Tasks.UpdateRange(children);
            }

            _context.Tasks.Add(entity);

            await _context.SaveChangesAsync();
            operRes.Result = await _context.Tasks.Select(SelectExpression).FirstOrDefaultAsync(e => e.DescriptionId == description.Id);
            return operRes;
        }

        private static readonly Expression<Func<_Task, WorkItemDTO>> SelectExpression = e => new TaskInfoDTO
        {
            AssignedTo = e.AssignedToId == null? null : new UserDTO
            {
                Avatar = e.AssignedTo.Avatar,
                Email = e.AssignedTo.Email,
                FullName = e.AssignedTo.FullName,
                Id = e.AssignedToId.Value,
                UserName = e.AssignedTo.UserName
            },
            Code = e.Type.Code(),
            DescriptionId = e.DescriptionId,         
            Name = e.Name,
            Number = e.Number,
            Status = e.Status,
            Children = e.Children.Select(a => new ItemDTO
            {
                Code = a.Type.Code(),
                Name = a.Name,
                Number = a.Number,
                DescriptionId = a.DescriptionId
            }),
            Parent = e.Parent == null ? null : new ItemDTO
            {
                Code = WorkItemType.UserStory.Code(),
                Name = e.Parent.Name,
                Number = e.Parent.Number,
                DescriptionId = e.Parent.DescriptionId
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
