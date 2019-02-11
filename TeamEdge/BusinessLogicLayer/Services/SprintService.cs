using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
            await _validationService.ValidateProjectAccess(model.ProjectId, model.CreatorId, e => e.CanWrite);

            var entity = _mapper.Map<Sprint>(model);
            entity.DateOfCreation = DateTime.Now;

            var operRes = new OperationResult<SprintDTO>(true);

            var checkStories = await _validationService.CheckChildren<UserStory>(model.UserStories, model.ProjectId);
            operRes.Plus(checkStories);

            var checkTasks = await _validationService.CheckChildren<_Task>(model.Tasks, model.ProjectId);
            operRes.Plus(checkTasks);

            _context.Sprints.Add(entity);
            if (checkStories.Result != null)
            {
                var stories = checkStories.Result.Select(e => { e.SprintId = entity.Id; return e; });
                _context.UserStories.UpdateRange(stories);
            }
            if(checkTasks.Result != null)
            {
                var tasks = checkTasks.Result.Select(e => { e.SprintId = entity.Id; return e; });
                _context.Tasks.UpdateRange(tasks);
            }
            await _context.SaveChangesAsync();
            operRes.Result = new SprintDTO
            {
                Id = entity.Id,
                Name = entity.Name,
                Stories = checkStories.Result==null?0 : checkStories.Result.Count(),
                Tasks = checkTasks.Result==null?0: checkStories.Result.Count()
            };
            return operRes;
        }

        public async Task AddItemToSprint(int userId, int descId, int sprintId)
        {
            var project = await _context.Sprints.Where(e => e.Id == sprintId).Select(e => new { e.ProjectId }).FirstOrDefaultAsync();
            if (project == null)
                throw new NotFoundException("sprint_nf");
            await _validationService.ValidateProjectAccess(project.ProjectId, userId);

            var entity = await
                ((IQueryable<BaseWorkItem>)_context.UserStories
                    .Where(e => e.Description.ProjectId == project.ProjectId))
                .Concat(_context.Tasks
                    .Where(e => e.Description.ProjectId == project.ProjectId)).FirstOrDefaultAsync(e=>e.DescriptionId == descId);
            if (entity == null)
                throw new NotFoundException("item_nf");

            switch (entity)
            {
                case _Task task:
                    {
                        task.SprintId = sprintId;
                        _context.Tasks.Update(task);
                        break;
                    }
                case UserStory story:
                    {
                        story.SprintId = sprintId;
                        _context.UserStories.Update(story);
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException();
                    }
            }

            await _context.SaveChangesAsync();
        } 

        public async Task<IEnumerable<SprintDTO>> GetSprintsForProject(int userId, int projectId)
        {
            await _validationService.ValidateProjectAccess(projectId, userId);

            return await _context.Sprints.Where(e => e.ProjectId == projectId).Select(e=>new SprintDTO
            {
                Id = e.Id,
                Stories = e.UserStories.Count(),
                Tasks = e.Tasks.Count + e.UserStories.Sum(u=>u.Children.Count()),
                Name = e.Name
            })
            .ToListAsync();
        }
    }
}
