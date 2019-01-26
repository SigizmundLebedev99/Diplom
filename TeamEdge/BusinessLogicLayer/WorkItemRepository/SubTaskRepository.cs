using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class SubTaskRepository : WorkItemRepository
    {
        public SubTaskRepository(IServiceProvider provider) : base(provider) { }

        public override async Task<OperationResult<WorkItemDTO>> CreateWorkItem(WorkItemDescription description, CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            var entity = _mapper.Map<SubTask>(model);

            if (model.ParentId != null)
                operRes.Plus(await CheckParent<_Task>(model.ProjectId, model.ParentId.Value));

            if (!operRes.Succeded)
                return operRes;
            entity.Number = await GetNumber<SubTask>(model.ProjectId);
            entity.DescriptionId = description.Id;
            _context.SubTasks.Add(entity);

            await _context.SaveChangesAsync();
            operRes.Result = await _context.SubTasks.Select(SelectExpression).FirstOrDefaultAsync(e => e.DescriptionId == description.Id);
            return operRes;
        }

        public override Task<WorkItemDTO> GetWorkItem(string code, int number, int project)
        {
            return _context.SubTasks.Where(e => e.Description.ProjectId == project && e.Number == number)
               .Select(SelectExpression).FirstOrDefaultAsync();
        }

        public override IQueryable<ItemDTO> GetItems(GetItemsDTO model)
        {
            var filter = WorkItemHelper.GetFilter<SubTask>(model);
            return _context.SubTasks.Where(filter).Select(WorkItemHelper.ItemDTOSelector);
        }

        public override Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int number, CreateWorkItemDTO model)
        {
            throw new NotImplementedException();
        }

        private static readonly Expression<Func<SubTask, WorkItemDTO>> SelectExpression = e => new TaskInfoDTO
        {
            AssignedTo = e.AssignedToId == null ? null : new UserDTO
            {
                Avatar = e.AssignedTo.Avatar,
                Email = e.AssignedTo.Email,
                FullName = e.AssignedTo.FullName,
                Id = e.AssignedToId.Value,
                UserName = e.AssignedTo.UserName
            },
            Code = WorkItemType.SubTask.Code(),
            DescriptionId = e.DescriptionId,
            Name = e.Name,
            Number = e.Number,
            Status = e.Status,
            Parent = e.Parent == null ? null : new ItemDTO
            {
                Code = e.Parent.Type.Code(),
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
