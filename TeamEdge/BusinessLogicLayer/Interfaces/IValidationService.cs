using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IValidationService
    {
        Task<OperationResult> ValidateBranches(string[] branches, string repositoryPath);
        Task<OperationResult> ValidateFileIds(int[] fileIds, int projectId);
        Task ValidateProject(int projId, int userId);
        Task ValidateProject(int projId, int userId, Expression<Func<UserProject, bool>> filter);
    }
}
