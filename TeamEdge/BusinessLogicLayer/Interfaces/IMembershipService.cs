using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IMembershipService
    {
        Task<InviteDTO> CreateInvite(CreateInviteDTO model);
        Task<OperationResult> JoinProject(JoinProjectDTO model);
        Task UpdatePartisipantStatus(ChangeStatusDTO model);
        Task DeletePartisipant(DeletePartisipantDTO model);
        Task<OperationResult> LeaveProject(int userId, int projectId);
    }
}
