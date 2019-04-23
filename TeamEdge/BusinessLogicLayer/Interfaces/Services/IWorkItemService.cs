using System.Collections.Generic;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IWorkItemService
    {
        Task<WorkItemDTO> GetWorkItem(int projId, int fromUserId, string code, int number);
        Task<OperationResult<WorkItemDTO>> CreateWorkItem(CreateWorkItemDTO model);
        Task<IEnumerable<ItemDTO>> GetListOfItems(GetItemsDTO getItemsDTO);
        Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int number, CreateWorkItemDTO model);
        Task<IEnumerable<ItemDTO>> GetTasksForUser(int projectId, int userId);
        Task<ItemDTO> GetDenseWorkItem(string code, int number, int projectId, int userId);
        Task<IEnumerable<ItemDTO>> GetBacklog(int projectId);
        Task<IEnumerable<ItemDTO>> GetWorkItemsForSprint(int projectId, int userId);
    }
}
