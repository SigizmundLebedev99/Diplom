﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.Models;

namespace TeamEdge.WebLayer.Controllers
{
    [Route("api/project")]
    [Authorize]
    public class ProjectController : Controller
    {
        readonly IProjectService _projectService;
        readonly IMapper _mapper;
        readonly IFileWorkService _fileWorkService;

        public ProjectController(IProjectService projectService, IMapper mapper, IFileWorkService fwservice)
        {
            _projectService = projectService;
            _mapper = mapper;
            _fileWorkService = fwservice;
        }

        /// <summary>
        /// Get projects and invites for user
        /// </summary>
        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(ProjectsForUserDTO))]
        public async Task<IActionResult> GetProjectsForUser(int userId)
        {
            var result = await _projectService.GetProjectsForUserAsync(userId);
            if (result == null)
                throw new NotFoundException("user_nf");
            return Ok(result);
        }

        /// <summary>
        /// Create new project
        /// </summary>
        [HttpPost]
        [ProducesResponseType(200, Type = typeof(ProjectDTO))]
        public async Task<IActionResult> CreateProject([FromForm]CreateProjectVM model)
        {
            var dto = _mapper.Map<CreateProjectDTO>(model);
            dto.UserId = User.Id();
            dto.Logo = _fileWorkService.SavePhoto(model.Logo);
            var result = await _projectService.CreateProject(dto);
            return Ok(result);
        }

        /// <summary>
        /// Get infomation about current project
        /// </summary>
        [HttpGet("{projectId}")]
        [ProducesResponseType(200, Type = typeof(ProjectInfoDTO))]
        public async Task<IActionResult> GetProjectInfo(int projectId)
        {
            var result = await _projectService.GetProjectInfo(projectId, User.Id());
            return Ok(result);
        }

        /// <summary>
        /// Update info about project
        /// </summary>
        [HttpPut("{projectId}")]
        [ProducesResponseType(200, Type = typeof(ProjectDTO))]
        public async Task<IActionResult> UpdateProjectInfo(int projectId, [FromForm]CreateProjectVM model)
        {
            var dto = _mapper.Map<CreateProjectDTO>(model);
            dto.UserId = User.Id();
            var result = await _projectService.UpdateProject(projectId, dto);
            return Ok(result);
        }
    }
}
