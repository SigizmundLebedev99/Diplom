using LibGit2Sharp;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class RepositoryService : IRepositoryService
    {
        readonly TeamEdgeDbContext _context;
        readonly PathParams _params;
        readonly IValidationService _validationService;

        public RepositoryService(TeamEdgeDbContext context, PathParams parameters, IValidationService validationService)
        {
            _context = context;
            _params = parameters;
            _validationService = validationService;
        }

        public async Task<int> CreateRepository(CreateRepositoryDTO model)
        {
            var projectName = (await _context.Projects.FirstOrDefaultAsync(p => p.Id == model.ProjectId && p.RepositoryId == null))?.Name;
            if (string.IsNullOrEmpty(projectName))
                throw new Infrastructure.NotFoundException("project_nf");
            if (!await _context
                .UserProjects
                .AnyAsync(u => u.UserId == model.UserId && u.ProjectId == model.ProjectId && u.ProjRole == ProjectAccessLevel.Administer))
                throw new UnauthorizedException("user_inv");

            var newRepo = new _Repository
            {
                CreatorId = model.UserId,
                DateOfCreation = DateTime.Now,
                ProjectId = model.ProjectId
            };
            _context.Repositories.Add(newRepo);      

            string path = Path.Combine(_params.RepositoriesDirPath, projectName);
            if (!Directory.Exists(path))
            {
                Repository.Init(path, true);
            }
            else
            {
                throw new InvalidOperationException("Folder with same name already exist");
            }

            await _context.SaveChangesAsync();

            return newRepo.Id;
        }

        public async Task CreateBranch(CreateBranchDTO model)
        {
            var repo = await ValidateInput(model.ProjectId, model.UserId, e=>e.CanPush);

            if (!string.IsNullOrEmpty(model.FromSha))
            {
                var commit = repo.Commits.FirstOrDefault(e => e.Sha == model.FromSha);
                repo.CreateBranch(model.Branch, commit);
            }
            else
            {
                repo.CreateBranch(model.Branch);
            }
        }

        public async Task<IEnumerable<string>> GetBranches(GetBranchesDTO model)
        {
            var repo = await ValidateInput(model.ProjectId, model.UserId);
            IEnumerable<Branch> branches = repo.Branches;

            if (model.FromUserId != null)
            {
                string name = await _context.UserProjects
                    .Where(e => e.ProjectId == model.ProjectId
                    && e.UserId == model.FromUserId)
                    .Select(e => e.User.UserName)
                    .FirstOrDefaultAsync();

                if (!string.IsNullOrEmpty(name))
                {
                    branches = branches.Where(e => e.Commits.Any(c => c.Committer.Name == name));
                }
            }

            if (!string.IsNullOrEmpty(model.Search))
            {
                branches = branches.Where(e => e.RemoteName.ToUpper().Contains(model.Search.ToUpper()));
            }

            return branches.Select(e => e.RemoteName);
        }

        public Task<bool> HasPermission(string username, string repositoryName, Expression<Func<UserProject, bool>> predicate)
        {
            return _context
                 .Projects
                 .FirstOrDefault(p => p.Name == repositoryName)
                 .Users
                 .AsQueryable()
                 .Where(predicate)
                 .AnyAsync(u => u.User.UserName == username);
        }

        private async Task<Repository> ValidateInput(int projectId, int userId, Expression<Func<UserProject, bool>> filter = null)
        {
            var project = await _context.Projects
                .Where(e => e.Id == projectId)
                .Select(e => e.Name).FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(project))
                throw new Infrastructure.NotFoundException("project_nf");

            if (filter != null)
            {
                if (!await _context.UserProjects.Where(filter).AnyAsync(e => e.ProjectId == projectId && e.UserId == userId))
                    throw new UnauthorizedException();
            }
            else
            {
                if (!await _context.UserProjects.AnyAsync(e => e.ProjectId == projectId && e.UserId == userId))
                    throw new UnauthorizedException();
            }

            string path = Path.Combine(_params.RepositoriesDirPath, project);
            if (!Repository.IsValid(path))
                throw new Infrastructure.NotFoundException("repo_nf");
            return new Repository(path);
        }
    }
}
