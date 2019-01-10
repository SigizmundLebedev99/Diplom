using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
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

        public WorkItemService(TeamEdgeDbContext context, IMapper mapper, IValidationService validationService)
        {
            _context = context;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task<WorkItemDTO> GetWorkItem(int projId, int fromUserId, string code, int number)
        {
            await _validationService.ValidateProject(projId, fromUserId);

            var result = await GetRepository(code).GetWorkItem(number, projId);
            if (result == null)
                throw new NotFoundException("number_nf");

            return result;
        }

        public async Task<OperationResult<WorkItemDTO>> CreateWorkItem(CreateWorkItemDTO model)
        {
            var operRes = new OperationResult<WorkItemDTO>(true);
            string project = await _context.Projects.Where(p => p.Id == model.ProjectId).Select(e => e.Name).FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(project))
                throw new NotFoundException("project_nf");
            if (!await _context.UserProjects.AnyAsync(p => p.UserId == model.CreatorId && p.ProjectId == model.ProjectId))
                throw new UnauthorizedException();

            
            operRes.Plus(await _validationService.ValidateBranches(model.Branches, project));
            operRes.Plus(await _validationService.ValidateFileIds(model.FileIds, model.ProjectId));

            if (!operRes.Succeded)
                return operRes;
            var description = _mapper.Map<WorkItemDescription>(model);
            description.DateOfCreation = DateTime.Now;
            _context.WorkItemDescriptions.Add(description);
            
            return await GetRepository(model.Code).CreateWorkItem(description, model);
        }

        //public async Task<IEnumerable<WorkItemDTO>> GetItemsForProject(GetWorkItemsDTO model)
        //{
        //    await _validationService.ValidateProject(model.ProjectId, model.UserId);

        //    var attr = EnumElements.FirstOrDefault(e => e.Code == model.Code);
            



        //    //_context.Epicks.Select(e => new ItemWithChildrenDTO
        //    //{
        //    //    Children = e.Children.Select(a=>new ItemWithChildrenDTO
        //    //    {
        //    //        Children = a.Children.Select(b=>new ItemWithChildrenDTO
        //    //        {
        //    //            Children = b.Children.Select(c=>new ItemWithChildrenDTO
        //    //            {
        //    //                e
        //    //            })
        //    //        })
        //    //    })
        //    //});
        //}

        static Expression<Func<BaseWorkItem, bool>> FilterExpr(int projectId)
        {
            return (i) => i.Description.ProjectId == projectId;
        }        

        static WorkItemService()
        {
            EnumElements = typeof(WorkItemType)
                    .GetMembers()
                    .Select(e => (WorkItemAttribute)e.GetCustomAttribute(typeof(WorkItemAttribute)))
                .Concat(typeof(TaskType)
                    .GetMembers()
                    .Select(e => (WorkItemAttribute)e.GetCustomAttribute(typeof(WorkItemAttribute))))
                    .Where(e=>e!=null)
                    .ToArray();
        }

        private static IEnumerable<WorkItemAttribute> EnumElements;

        private WorkItemRepository GetRepository(string code)
        {
            var attr =  EnumElements.FirstOrDefault(e => e.Code == code);
            if (attr == null)
                throw new NotFoundException("code_inv");
            return (WorkItemRepository)Activator.CreateInstance(attr.FactoryType, _context, _mapper);
        }
    }
}
