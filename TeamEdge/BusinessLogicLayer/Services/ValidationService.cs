﻿using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.Models;
using LibGit2Sharp;
using System.IO;
using TeamEdge.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System;
using TeamEdge.DAL.Models;

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
                foreach(var f in files.Where(e => !fileIds.Contains(e.Id)))
                {
                    operRes.AddErrorMessage("file_inv", f.Id);
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

    }
}
