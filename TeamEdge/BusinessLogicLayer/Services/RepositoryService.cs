using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
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

        public RepositoryService(TeamEdgeDbContext context, PathParams parameters)
        {
            _context = context;
            _params = parameters;
        }

        public async Task<int> CreateRepository(CreateRepositoryDTO model)
        {
            var projectName = (await _context.Projects.FirstOrDefaultAsync(p => p.Id == model.ProjectId && p.RepositoryId == null))?.Name;
            if (string.IsNullOrEmpty(projectName))
                throw new NotFoundException("project_nf");
            if (!await _context
                .UserProjects
                .AnyAsync(u => u.UserId == model.UserId && u.ProjectId == model.ProjectId && u.ProjRole == ProjectAccessLevel.Administer))
                throw new UnauthorizedException("user_inv");

            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var newRepo = new Repository
                    {
                        CreatorId = model.UserId,
                        DateOfCreation = DateTime.Now,
                        ProjectId = model.ProjectId
                    };
                    _context.Repositories.Add(newRepo);

                    string path = Path.Combine(_params.RepositoriesDirPath, projectName);
                    if (!Directory.Exists(path))
                    {
                        LibGit2Sharp.Repository.Init(path, true);
                    }
                    else
                    {
                        throw new InvalidOperationException("Folder with same name already exist");
                    }

                    await _context.SaveChangesAsync();
                    return newRepo.Id;
                }
                catch(Exception ex)
                {
                    transaction.Dispose();
                    throw;
                }
            }
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
    }
}
