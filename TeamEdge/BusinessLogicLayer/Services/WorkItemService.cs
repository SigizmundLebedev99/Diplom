using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            await _validationService.ValidateProject(model.ProjectId, model.UserId);
            bool ofType = !string.IsNullOrEmpty(model.Code);

            IQueryable<ItemDTO> query = null;

            if (ofType)
            {
                query = GetRepository(model.Code).GetItems(model);
            }
            else
                query = _context.GetWorkItems(WorkItemHelper.GetFilter<BaseWorkItem>(model), WorkItemHelper.ItemDTOSelector);

            return await query.ToListAsync();
        }

        public async Task<WorkItemDTO> GetWorkItem(int projId, int fromUserId, string code, int number)
        {
            await _validationService.ValidateProject(projId, fromUserId);

            var result = await GetRepository(code).GetWorkItem(code, number, projId);
            if (result == null)
                throw new NotFoundException("number_nf");

            return result;
        }

        public async Task<OperationResult<WorkItemDTO>> CreateWorkItem(CreateWorkItemDTO model)
        {
            var operRes = await ValidateItemDTO(model);
            if (!operRes.Succeded)
                return operRes;
            var description = _mapper.Map<WorkItemDescription>(model);
            description.DateOfCreation = DateTime.Now;
            _context.WorkItemDescriptions.Add(description);
            
            return await GetRepository(model.Code).CreateWorkItem(description, model);
        }

        public async Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int number, CreateWorkItemDTO model)
        {
            var operRes = await ValidateItemDTO(model);

            return await GetRepository(model.Code).UpdateWorkItem(number, model);
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

        private async Task<OperationResult<WorkItemDTO>> ValidateItemDTO(CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            string project = await _context.Projects.Where(p => p.Id == model.ProjectId).Select(e => e.Name).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(project))
                throw new NotFoundException("project_nf");
            if (!await _context.UserProjects.AnyAsync(p => p.UserId == model.CreatorId && p.ProjectId == model.ProjectId))
                throw new UnauthorizedException();


            operRes.Plus(await _validationService.ValidateBranches(model.Branches, project));
            operRes.Plus(await _validationService.ValidateFileIds(model.FileIds, model.ProjectId));

            return operRes;
        }
        #endregion
    }
}
