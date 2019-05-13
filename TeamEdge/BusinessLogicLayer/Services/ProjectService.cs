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
    public class ProjectService : IProjectService
    {
        readonly TeamEdgeDbContext _context;
        readonly IMapper _mapper;
        IValidationService _validationService;

        public ProjectService(TeamEdgeDbContext context, IMapper mapper, IValidationService validationService)
        {
            _context = context;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task<ProjectDTO> CreateProject(CreateProjectDTO model)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == model.UserId))
                throw new UnauthorizedException();

            var entity = _mapper.Map<Project>(model);
            entity.DateOfCreation = DateTime.Now;
            entity.Users = new UserProject[]
            {
                new UserProject{UserId = model.UserId, ProjRole = ProjectAccessLevel.Administer}
            };
            var result = _context.Projects.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProjectDTO>(result.Entity);
        }

        public async Task<IEnumerable<FileDTO>> GetFilesForProject(int projectId, int userId)
        {
            await _validationService.ValidateProjectAccess(projectId, userId);
            return await _context.Files.Where(e => e.ProjectId == projectId).Select(e => new FileDTO
            {
                Id = e.Id,
                FileName = e.FileName,
                CreatedBy = new UserLightDTO
                {
                    Avatar = e.Creator.Avatar,
                    Name = e.Creator.FullName,
                    Id = e.CreatorId
                },
                DateOfCreation = e.DateOfCreation
            }).ToArrayAsync();
        }

        public async Task<ProjectInfoDTO> GetProjectInfo(int projectId, int userId)
        {
            var up = await _context.UserProjects.Where(u => u.UserId == userId && u.ProjectId == projectId).Select(e => new { e.ProjRole }).FirstOrDefaultAsync();
            if (up == null)
                throw new UnauthorizedException();

            return await _context.Projects.Select(p => new ProjectInfoDTO
            {
                Partisipants = p.Users.Select(u => new UserDTO
                {
                    Id = u.UserId,
                    Email = u.User.Email,
                    FullName = u.User.FullName,
                    UserName = u.User.UserName,
                    Avatar = u.User.Avatar
                }).ToArray(),
                Id = p.Id,
                Logo = p.Logo,
                Name = p.Name,
                AccessLevel = up.ProjRole
            }).FirstOrDefaultAsync(e=>e.Id == projectId);
        }

        public async Task<IEnumerable<InviteDTO>> GetInvitesToProject(int userId, int projectId)
        {
            await _validationService.ValidateProjectAccess(projectId, userId, u => u.IsAdmin);
            var result = await _context.Invites.Where(e => e.ProjectId == projectId && !e.IsAccepted).Select(i => new InviteDTO
            {
                FromEmail = i.Creator.Email,
                FromFullName = i.Creator.FullName,
                FromId = i.CreatorId,
                FromAvatar = i.Creator.Avatar,
                InviteId = i.Id,
                Logo = i.Project.Logo,
                ProjectName = i.Project.Name,
                ProjectId = i.ProjectId
            }).ToListAsync();

            return result;
        }

        public async Task<ProjectsForUserDTO> GetProjectsForUserAsync(int userId)
        {
            if (!await _context.Users.AnyAsync(u => u.Id == userId))
                throw new UnauthorizedException();
            var tasks = await _context.Tasks.Where(t => t.AssignedToId == userId && (t.Status == WorkItemStatus.Active || t.Status == WorkItemStatus.New))
                .Select(t => new { t.Description.ProjectId }).ToArrayAsync();
            return await _context.Users
                .Where(u=>u.Id == userId)
                .Select(u => new ProjectsForUserDTO
                {
                    Projects = u.UserProjects.Select(p => new ProjectForUserDTO()
                    {
                        Id = p.ProjectId,
                        Logo = p.Project.Logo,
                        Name = p.Project.Name,
                        AccessStatus = p.ProjRole,
                        HasTasks = tasks.Any(e=>e.ProjectId == p.ProjectId),
                        UsersCount = p.Project.Users.Count()
                    }),
                    Invites = u.Invites.Where(i=>!i.IsAccepted).Select(i => new InviteForUserDTO
                    {
                        FromEmail = i.Creator.Email,
                        FromFullName = i.Creator.FullName,
                        FromId = i.CreatorId,
                        FromAvatar = i.Creator.Avatar,
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
            newProject.DateOfCreation = previous.DateOfCreation;
            _context.Projects.Update(newProject);
            await _context.SaveChangesAsync();
            return _mapper.Map<ProjectDTO>(newProject);
        }
    }
}
