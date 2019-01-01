using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IRepositoryService
    {
        Task<int> CreateRepository(CreateRepositoryDTO model);
        Task<bool> HasPermission(string username, string repositoryName, Expression<Func<UserProject, bool>> predicate);
    }
}
