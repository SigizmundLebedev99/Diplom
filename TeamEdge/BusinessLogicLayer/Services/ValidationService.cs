using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using System.IO;
using TeamEdge.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using TeamEdge.DAL.Models;
using System.Collections.Generic;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class ValidationService : IValidationService
    {
        readonly PathParams _parameters;
        readonly TeamEdgeDbContext _context;

        public ValidationService(TeamEdgeDbContext context, PathParams parameters)
        {
            _parameters = parameters;
            _context = context;
        }

        public Task<OperationResult> ValidateBranches(string[] branches, string repositoryName)
        {
            return Task.Run(() =>
            {
                var operRes = new OperationResult(true);
                if (branches == null || branches.Length == 0)
                    return operRes;
                string path = Path.Combine(_parameters.RepositoriesDirPath, repositoryName);
                if (!LibGit2Sharp.Repository.IsValid(path))
                {
                    operRes.AddErrorMessage("repo_nf");
                    return operRes;
                }
                var repository = new LibGit2Sharp.Repository(path);
                var errors = branches.Where(b => !repository.Branches.Select(e => e.FriendlyName).Contains(b));
                if (errors.Count()>0)
                {
                    foreach (var b in errors)
                        operRes.AddErrorMessage("branch_nf", b);
                }
                return operRes;
            });
        }

        public async Task<OperationResult> ValidateFileIds(int[] fileIds, int projectId)
        {
            var operRes = new OperationResult(true);
            var files = await _context.Files
                .Where(e => fileIds.Contains(e.Id) && e.ProjectId == projectId).Select(e=> new { e.Id})
                .ToArrayAsync();

            if (files.Length < fileIds.Length)
            {
                foreach(var fid in fileIds.Where(i => !files.Any(f=>f.Id == i)))
                {
                    operRes.AddErrorMessage("file_inv", fid);
                }
            }

            return operRes;
        }

        public async Task ValidateProject(int projId, int userId)
        {
            if (!await _context.Projects.AnyAsync(p => p.Id == projId))
                throw new Infrastructure.NotFoundException("project_nf");
            if (!await _context.UserProjects.AnyAsync(p => p.UserId == userId && p.ProjectId == projId))
                throw new UnauthorizedException();
        }

        public async Task ValidateProject(int projId, int userId, Expression<Func<UserProject, bool>> filter)
        {
            if (!await _context.Projects.AnyAsync(p => p.Id == projId))
                throw new Infrastructure.NotFoundException("project_nf");
            if (!await _context.UserProjects.Where(filter).AnyAsync(p => p.UserId == userId && p.ProjectId == projId))
                throw new UnauthorizedException();
        }

        public async Task<OperationResult<IEnumerable<T>>> CheckChildren<T>(int[] childrenIds, int projectId) where T : BaseWorkItem
        {
            var operRes = new OperationResult<IEnumerable<T>>(true);
            T[] children = null;
            if (childrenIds != null && childrenIds.Length > 0)
            {
                children = await _context.Set<T>()
                    .Where(t => t.Description.ProjectId == projectId
                    && childrenIds.Contains(t.DescriptionId)).ToArrayAsync();

                if (children.Length < childrenIds.Distinct().Count())
                {
                    foreach (var i in childrenIds.Where(i => !children.Any(e => e.DescriptionId == i)))
                    {
                        operRes.AddErrorMessage("children_nf", i);
                    }
                }
                else
                {
                    operRes.Result = children;
                }
            }
            return operRes;
        }
    }
}
