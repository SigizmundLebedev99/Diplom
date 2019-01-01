using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (!await _context.Projects.AnyAsync(p => p.Id == projId))
                throw new NotFoundException("project_nf");
            if (!await _context.UserProjects.AnyAsync(p => p.UserId == fromUserId && p.ProjectId == projId))
                throw new UnauthorizedException();

            var result = await GetRepository(code).GetWorkItem(number, projId);
            if (result == null)
                throw new NotFoundException("number_nf");

            return result;
        }

        public async Task<OperationResult<WorkItemDTO>> CreateWorkItem(CreateWorkItemDTO model)
        {
            if (!await _context.Projects.AnyAsync(p => p.Id == model.ProjectId))
                throw new NotFoundException("project_nf");
            if (!await _context.UserProjects.AnyAsync(p => p.UserId == model.CreatorId && p.ProjectId == model.ProjectId))
                throw new UnauthorizedException();

            var description = _mapper.Map<WorkItemDescription>(model);
            
            return await GetRepository(model.Code).CreateWorkItem(description.Id, model);
        }

        static WorkItemService()
        {
            EnumElements = typeof(WorkItemType)
                    .GetMembers()
                    .Select(e => (WorkItemAttribute)e.GetCustomAttribute(typeof(WorkItemAttribute)))
                .Concat(typeof(TaskType)
                    .GetMembers()
                    .Select(e => (WorkItemAttribute)e.GetCustomAttribute(typeof(WorkItemAttribute))));
        }

        private static IEnumerable<WorkItemAttribute> EnumElements;

        private WorkItemRepository GetRepository(string code)
        {
            var attr =  EnumElements.FirstOrDefault(e => e.Code == code);
            if (attr == null)
                throw new NotFoundException("code_nf");
            return (WorkItemRepository)Activator.CreateInstance(attr.Type, _context, _mapper);
        }

    }
}
