using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TeamEdge.BusinessLogicLayer.Helpers;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class SprintService : ISprintService
    {
        readonly IMapper _mapper;
        readonly TeamEdgeDbContext _context;
        readonly IValidationService _validationService;

        public SprintService(IValidationService validationService, TeamEdgeDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
            _validationService = validationService;
        }

        public async Task<OperationResult<SprintDTO>> CreateSprint(CreateSprintDTO model)
        {
            await _validationService.ValidateProjectAccess(model.ProjectId, model.CreatorId, e => e.CanReview);

            var entity = _mapper.Map<Sprint>(model);
            entity.DateOfCreation = DateTime.Now;

            var operRes = new OperationResult<SprintDTO>(true);

            entity.Duration = TimeHelper.GetTimeSpanNumber(model.Days, model.Hours);
            if (!TimeHelper.CheckTimeConstraints(entity))
                operRes.AddErrorMessage("time_inv");

            _context.Sprints.Add(entity);

            await _context.SaveChangesAsync();
            operRes.Result = _mapper.Map<SprintDTO>(entity);
            return operRes;
        }

        public async Task<IEnumerable<SprintDTO>> GetSprintsForProject(int userId, int projectId)
        {
            await _validationService.ValidateProjectAccess(projectId, userId);

            return await _context.Sprints.Where(e => e.ProjectId == projectId).Select(e => new SprintDTO
            {
                Id = e.Id,
                Name = e.Name,
                StartDate = e.StartDate,
                Duration = e.Duration.ToTimeSpan(),
                EndDate = e.EndDate
            })
            .ToListAsync();
        }
    }
}
