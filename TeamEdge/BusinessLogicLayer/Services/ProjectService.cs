﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class ProjectService : IProjectService
    {
        readonly TeamEdgeDbContext _context;
        readonly IMapper _mapper;

        public ProjectService(TeamEdgeDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProjectDTO> CreateProject(CreateProjectDTO model)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == model.UserId))
                throw new UnauthorizedException();

            var entity = _mapper.Map<Project>(model);
            var result = _context.Projects.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProjectDTO>(result.Entity);
        }

        public async Task<ProjectInfoDTO> GetProjectInfo(int projectId, int userId)
        {
            if (!await _context.UserProjects.AnyAsync(u => u.UserId == userId && u.ProjectId == projectId))
                throw new UnauthorizedException();

            return await _context.Projects.Select(p => new ProjectInfoDTO
            {
                Partisipants = p.Users.Select(u => new UserDTO
                {
                    Id = u.UserId,
                    Email = u.User.Email,
                    FullName = u.User.FullName,
                    UserName = u.User.UserName
                }).ToArray(),
                Id = p.Id,
                Logo = p.Logo,
                Name = p.Name
            }).FirstOrDefaultAsync(e=>e.Id == projectId);
        }

        public async Task<ProjectsForUserDTO> GetProjectsForUserAsync(int userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new UnauthorizedException();

            return await _context.Users
                .Where(u=>u.Id == userId)
                .Select(u => new ProjectsForUserDTO
                {
                    Projects = u.UserProjects.Select(p => new ProjectDTO
                    {
                        Id = p.ProjectId,
                        Logo = p.Project.Logo,
                        Name = p.Project.Name
                    }),
                    Invites = u.Invites.Where(i=>!i.IsAccepted).Select(i => new InviteForUserDTO
                    {
                        FromEmail = i.Creator.Email,
                        FromFullName = i.Creator.FullName,
                        FromId = i.CreatorId,
                        InviteId = i.Id,
                        Logo = i.Project.Logo,
                        ProjectName = i.Project.Name,
                        ProjectId = i.ProjectId
                    })
                }).FirstOrDefaultAsync();
        }

        public async Task<ProjectDTO> UpdateProject(int id, CreateProjectDTO model)
        {
            var userProj = await _context
                .UserProjects
                .Include(u => u.Project)
                .FirstOrDefaultAsync(u => u.UserId == model.UserId && u.ProjectId == id && u.IsAdmin);
            if (userProj == null)
                throw new UnauthorizedException();

            var previous = userProj.Project;
            var newProject = _mapper.Map<Project>(model);
            newProject.Id = id;
            newProject.RepositoryId = previous.RepositoryId;
            newProject.DateOfCreation = previous.DateOfCreation;
            _context.Projects.Update(newProject);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProjectDTO>(newProject);
        }
    }
}
