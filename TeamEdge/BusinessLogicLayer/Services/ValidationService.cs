using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.BusinessLogicLayer.Interfaces;
using TeamEdge.Models;
using LibGit2Sharp;
using System.IO;

namespace TeamEdge.BusinessLogicLayer.Services
{
    public class ValidationService : IValidationService
    {
        readonly GitServiceParams _parameters;

        public ValidationService(GitServiceParams parameters)
        {
            _parameters = parameters;
        }

        public Task<OperationResult> ValidateBranches(string[] branches, string repositoryName)
        {
            return Task.Run(() =>
            {
                var operRes = new OperationResult(true);
                string path = Path.Combine(_parameters.RepositoriesDirPath, repositoryName);
                if (!Repository.IsValid(path))
                {
                    operRes.AddErrorMessage("repo_nf");
                    return operRes;
                }
                var repository = new Repository(path);
                var errors = branches.Where(b => !repository.Branches.Select(e => e.FriendlyName).Contains(b));
                if (errors.Count()>0)
                {
                    foreach (var b in errors)
                        operRes.AddErrorMessage("branch_nf", b);
                }
                return operRes;
            });
        }

        public Task<OperationResult> ValidateFiles(FileDTO[] files)
        {
            throw new NotImplementedException();
        }
    }
}
