using System.Collections.Generic;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IWorkItemService
    {
        Task<WorkItemDTO> GetWorkItem(int projId, int fromUserId, string code, int number);
        Task<OperationResult<WorkItemDTO>> CreateWorkItem(CreateWorkItemDTO model);
        Task<IEnumerable<ItemDTO>> GetListOfItems(GetItemsDTO getItemsDTO);
        Task<OperationResult<WorkItemDTO>> UpdateWorkItem(int descriptionId, CreateWorkItemDTO model);
    }
}
