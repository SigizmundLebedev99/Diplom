using System.Collections.Generic;
using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrastructure;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface ISprintService
    {
        Task<OperationResult<SprintDTO>> CreateSprint(CreateSprintDTO model);
        Task<IEnumerable<SprintDTO>> GetSprintsForProject(int userId, int projectId);
        Task AddItemToSprint(int userId, int descId, int sprintId);
    }
}
