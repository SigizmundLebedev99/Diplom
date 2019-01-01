using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IWorkItemService
    {
        Task<WorkItemDTO> GetWorkItem(int projId, int fromUserId, string code, int number);
        Task<OperationResult<WorkItemDTO>> CreateWorkItem(CreateWorkItemDTO model);
    }
}
