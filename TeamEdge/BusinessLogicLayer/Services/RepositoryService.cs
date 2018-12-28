using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using TeamEdge.BusinessLogicLayer.Git;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.DAL.Context;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class RepositoryService : IRepositoryService
    {
        readonly TeamEdgeDbContext _context;
        readonly GitServiceParams _params;

        public RepositoryService(TeamEdgeDbContext context, GitServiceParams parameters)
        {
            _context = context;
            _params = parameters;
        }

        public async Task<OperationResult> CreateRepository(int creatorId, CreateRepositoryDTO model)
        {
            var operRes = new OperationResult(true);
            var projectName = (await _context.Projects.FirstOrDefaultAsync(p => p.Id == model.ProjectId && p.RepositoryId == null))?.Name;
            if (string.IsNullOrEmpty(projectName))
                operRes.AddErrorMessage("project-nf", "");
            if(!await _context
                .UserProjects
                .AnyAsync(u=>u.UserId == creatorId && u.ProjectId == model.ProjectId && u.ProjRole == ProjectAccessLevel.Administer))
                operRes.AddErrorMessage("user-inv", "");

            if (!operRes.Succeded)
                return operRes;

            using (TransactionScope transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _context.Repositories.Add(new Repository
                    {
                        CreatorId = creatorId,
                        DateOfCreation = DateTime.Now,
                        ProjectId = model.ProjectId
                    });

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
                }
                catch(Exception ex)
                {
                    transaction.Dispose();
                    throw;
                }
            }

            return operRes;
        }

        public Task<bool> HasPermission(string username, string repositoryName, RepositoryAccessLevel requiredLevel)
        {
           return _context    
                .Projects
                .FirstOrDefault(p=>p.Name == repositoryName)
                .Users
                .AsQueryable()
                .AnyAsync(u => u.RepoRole == requiredLevel && u.User.UserName == username);
        }
    }
}
