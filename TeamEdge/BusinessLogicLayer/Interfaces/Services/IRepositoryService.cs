using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IRepositoryService
    {
        Task<int> CreateRepository(CreateRepositoryDTO model);
        Task<bool> HasPermission(string username, string repositoryName, Expression<Func<UserProject, bool>> predicate);
        Task<IEnumerable<string>> GetBranches(GetBranchesDTO getBranchesDTO);
        Task CreateBranch(CreateBranchDTO model);
    }
}
