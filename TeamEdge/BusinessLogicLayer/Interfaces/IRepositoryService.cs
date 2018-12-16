using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.DAL.Models;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IRepositoryService
    {
        Task<OperationResult> CreateRepository(int creatorId, CreateRepositoryDTO model);
        Task<bool> HasPermission(int userId, string repositoryName, RepositoryAccessLevel requiredLevel);
    }
}
