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

namespace TeamEdge.BusinessLogicLayer.Services.WorkItemFactory
{
    public class TaskRepository : WorkItemRepository
    {
        public TaskRepository(TeamEdgeDbContext context, IMapper mapper) : base(context, mapper) { }

        public override Task<WorkItemDTO> GetWorkItem(int number, int project)
        {
            return _context.Tasks
                .Where(e => e.Number == number && e.Description.ProjectId == project)
                .Select(SelectExpression)
                .FirstOrDefaultAsync();
        }

        public override Task<OperationResult<WorkItemDTO>> CreateWorkItem(int descriptionId, CreateWorkItemDTO model)
        {
            throw new NotImplementedException();
        }

        private static readonly Expression<Func<_Task, WorkItemDTO>> SelectExpression = e => new TaskInfoDTO
        {
            AssignedTo = e.AssignedTo == null? null : new UserDTO
            {
                Avatar = e.AssignedTo.Avatar,
                Email = e.AssignedTo.Email,
                FullName = e.AssignedTo.FullName,
                Id = e.AssignedToId,
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
                Number = a.Number
            }),
            Parent = e.Parent == null ? null : new ItemDTO
            {
                Code = WorkItemType.UserStory.Code(),
                Name = e.Parent.Name,
                Number = e.Parent.Number
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
                LastUpdateBy = e.Description.LastUpdater == null ? null : new UserDTO
                {
                    Avatar = e.Description.LastUpdater.Avatar,
                    Email = e.Description.LastUpdater.Email,
                    FullName = e.Description.LastUpdater.FullName,
                    Id = e.Description.LastUpdaterId,
                    UserName = e.Description.LastUpdater.UserName
                }
            }
        };
    }
}
