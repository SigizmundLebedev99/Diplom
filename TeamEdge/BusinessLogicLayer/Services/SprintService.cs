﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TeamEdge.BusinessLogicLayer.Infrastructure;
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
            await _validationService.ValidateProject(model.ProjectId, model.CreatorId, e => e.CanWrite);

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

        public async Task<IEnumerable<SprintDTO>> GetSprintsForProject(int userId, int projectId)
        {
            await _validationService.ValidateProject(projectId, userId);

            return await _context.Sprints.Where(e => e.ProjectId == projectId).Select(e=>new SprintDTO
            {
                Id = e.Id,
                Stories = e.UserStories.Count(),
                Tasks = e.Tasks.Count,
                Name = e.Name
            })
            .ToListAsync();
        }
    }
}
