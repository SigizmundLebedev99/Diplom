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
            await _validationService.ValidateProjectAccess(model.ProjectId, model.CreatorId);

            var entity = _mapper.Map<Sprint>(model);
            entity.DateOfCreation = DateTime.Now;
            var set = _context.Sprints.Where(e => e.ProjectId == model.ProjectId);
            if (await set.AnyAsync())
            {
                var number = await set
                    .Select(e => e.Number)
                    .MaxAsync();
                entity.Number = number + 1;
            }
            else
                entity.Number = 1;
            var operRes = new OperationResult<SprintDTO>(true);
            
            _context.Sprints.Add(entity);

            var tasks = await _context.Tasks.Where(e => model.Tasks.Contains(e.DescriptionId)).ToListAsync();
            var stories = await _context.UserStories.Where(e => model.UserStories.Contains(e.DescriptionId)).ToListAsync();
            foreach (var t in tasks)
                t.SprintId = entity.Id;
            foreach (var s in stories)
                s.SprintId = entity.Id;

            _context.Tasks.UpdateRange(tasks);
            _context.UserStories.UpdateRange(stories);

            await _context.SaveChangesAsync();
            operRes.Result = _mapper.Map<SprintDTO>(entity);
            return operRes;
        }

        public async Task<IEnumerable<SprintDTO>> GetSprintsForProject(int userId, int projectId)
        {
            await _validationService.ValidateProjectAccess(projectId, userId);

            return await _context.Sprints.Where(e => e.ProjectId == projectId).Select(e => new SprintDTO
            {
                Number = e.Number,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                ProjectId = e.ProjectId
            })
            .ToListAsync();
        }

        public async Task UpdateSprint(UpdateSprintDTO model)
        {
            await _validationService.ValidateProjectAccess(model.ProjectId, model.UserId);
            var entity = await _context.Sprints.FirstOrDefaultAsync(e => e.ProjectId == model.ProjectId && e.Number == model.Number);
            entity.StartDate = model.StartDate;
            entity.EndDate = model.EndDate;
            if (entity == null)
                throw new NotFoundException("sprint_nf");
            var tasks = await _context.Tasks.Where(e => model.Tasks.Contains(e.DescriptionId)).ToListAsync();
            var stories = await _context.UserStories.Where(e => model.UserStories.Contains(e.DescriptionId)).ToListAsync();
            foreach (var t in tasks)
                t.SprintId = entity.Id;
            foreach (var s in stories)
                s.SprintId = entity.Id;
            _context.Sprints.Update(entity);
            _context.Tasks.UpdateRange(tasks);
            _context.UserStories.UpdateRange(stories);

            await _context.SaveChangesAsync();
        }
    }
}
