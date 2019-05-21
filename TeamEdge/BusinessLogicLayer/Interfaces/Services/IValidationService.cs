using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IValidationService
    {
        Task<OperationResult> ValidateFileIds(int[] fileIds, int projectId);
        Task ValidateProjectAccess(int projId, int userId);
        Task ValidateProjectAccess(int projId, int userId, Expression<Func<UserProject, bool>> filter);
        Task<OperationResult<IEnumerable<T>>> CheckChildren<T>(int[] childrenIds, int projectId) where T : BaseWorkItem;
    }
}
