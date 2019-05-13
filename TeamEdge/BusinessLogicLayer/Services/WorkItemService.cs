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
    public class WorkItemService : IWorkItemService
    {
        readonly TeamEdgeDbContext _context;
        readonly IMapper _mapper;
        readonly IValidationService _validationService;
        readonly IServiceProvider _provider;

        public WorkItemService(TeamEdgeDbContext context, IMapper mapper, IValidationService validationService, IServiceProvider provider)
        {
            _context = context;
            _mapper = mapper;
            _validationService = validationService;
            _provider = provider;
        }

        public async Task<IEnumerable<ItemDTO>> GetListOfItems(GetItemsDTO model)
        {
            await _validationService.ValidateProjectAccess(model.ProjectId, model.UserId);
            bool ofType = !string.IsNullOrEmpty(model.Code);

            IQueryable<ItemDTO> query = null;

            if (ofType)
            {
                query = GetRepository(model.Code).GetItems(model);
            }
            else
                query = _context.GetWorkItems(WorkItemHelper.GetFilter<IBaseWorkItem>(model), WorkItemHelper.ItemDTOSelector);

            return await query.ToListAsync();
        }

        public async Task<WorkItemDTO> GetWorkItem(int projId, int fromUserId, string code, int number)
        {
            await _validationService.ValidateProjectAccess(projId, fromUserId);

            var result = await GetRepository(code).GetWorkItem(code, number, projId);
            if (result == null)
                throw new NotFoundException("number_nf");

            return result;
        }

        public async Task<ItemDTO> GetDenseWorkItem(string code, int number, int projectId,  int userId)
        {
            await _validationService.ValidateProjectAccess(projectId, userId);
            return await GetRepository(code).GetDenseWorkItem(code, number, projectId);
        }

        public async Task<OperationResult<WorkItemDTO>> CreateWorkItem(CreateWorkItemDTO model)
        {
            var (operRes, userProj) = await ValidateItemDTO(model);
            if (!operRes.Succeded)
                return operRes; 
            var description = _mapper.Map<WorkItemDescription>(model);
            description.Subscribers = new List<Subscribe>() { new Subscribe() { SubscriberId = model.CreatorId } };
            description.DateOfCreation = DateTime.Now;
            _context.WorkItemDescriptions.Add(description);
            
            return await GetRepository(model.Code).CreateWorkItem(description, model, userProj);
        }

        public async Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int number, CreateWorkItemDTO model)
        {
            var (operRes,userProj) = await ValidateItemDTO(model);
            if (!operRes.Succeded)
                return operRes;
            return await GetRepository(model.Code).UpdateWorkItem(number, model);
        }

        public async Task<IEnumerable<ItemDTO>> GetTasksForUser(int projectId, int userId)
        {
            await _validationService.ValidateProjectAccess(projectId, userId);
            return await _context.Tasks.Where(e => e.AssignedToId == userId && e.Description.ProjectId == projectId).Select(WorkItemHelper.ItemDTOSelector)
                .Concat(_context.SubTasks.Where(e => e.AssignedToId == userId && e.Description.ProjectId == projectId).Select(WorkItemHelper.ItemDTOSelector)).ToListAsync();
        }

        public async Task<IEnumerable<ItemDTO>> GetBacklog(int projectId)
        {
            Expression<Func<IBaseWorkItemWithParent, bool>> filter = e => e.Description.ProjectId == projectId;
            return await _context.Tasks.Where(e=>e.Description.ProjectId == projectId).Select(item => new ItemForBacklogWithSprintDTO
                {
                    Code = item.Code,
                    DescriptionId = item.DescriptionId,
                    Name = item.Name,
                    Number = item.Number,
                    Status = item.Status,
                    ParentId = item.ParentId??item.EpickId,
                    SprintNumber = item.Sprint.Number
                })
                .Concat(_context.UserStories.Where(e => e.Description.ProjectId == projectId).Select(item => new ItemForBacklogWithSprintDTO
                {
                    Code = "STORY",
                    DescriptionId = item.DescriptionId,
                    Name = item.Name,
                    Number = item.Number,
                    ParentId = item.ParentId,
                    SprintNumber = item.Sprint.Number
                }))
                .Concat(_context.Epics.Where(e=>e.Description.ProjectId == projectId).Select(WorkItemHelper.ItemDTOSelector))
                .Concat(_context.SubTasks.Where(filter).Select(WorkItemHelper.ItemBacklogDTOSelector)).ToListAsync();
        }

        public async Task<IEnumerable<ItemDTO>> GetWorkItemsForSprint(int projectId, int userId)
        {
            await _validationService.ValidateProjectAccess(projectId, userId);
            return await _context.UserStories.Where(e => e.Description.ProjectId == projectId && e.SprintId == null).Select(WorkItemHelper.ItemDTOSelector)
                .Concat(_context.Tasks.Where(e => e.Description.ProjectId == projectId && e.ParentId == null && e.SprintId == null)
                .Select(WorkItemHelper.ItemDTOSelector)).ToListAsync();
        }

        #region Privates
        private WorkItemRepository GetRepository(string code)
        {
            var attr = GetAttribute(code);
            return (WorkItemRepository)Activator.CreateInstance(attr.FactoryType, _provider);
        }  

        private WorkItemAttribute GetAttribute(string code)
        {
            var attr = WorkItemFactory.GetAttributeInstanse(code);
            if (attr == null)
                throw new NotFoundException("code_inv");
            return attr;
        }

        private async Task<(OperationResult<WorkItemDTO> operRes, UserProject userProj)> ValidateItemDTO(CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            var userProj = await _context.UserProjects.Where(p => p.ProjectId == model.ProjectId && p.UserId == model.CreatorId).FirstOrDefaultAsync();
            if (!await _context.Projects.AnyAsync(e=>e.Id == model.ProjectId))
                throw new NotFoundException("project_nf");
            if (userProj == null)
                throw new UnauthorizedException();

            operRes.Plus(await _validationService.ValidateFileIds(model.FileIds, model.ProjectId));

            return (operRes,userProj);
        }

        
        #endregion
    }
}
