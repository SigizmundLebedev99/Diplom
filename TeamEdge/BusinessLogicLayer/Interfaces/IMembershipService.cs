using System.Threading.Tasks;
using TeamEdge.BusinessLogicLayer.Infrostructure;
using TeamEdge.Models;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface IMembershipService
    {
        Task<OperationResult> SendInvite(CreateInviteDTO model);
        Task<OperationResult> JoinProject(JoinProjectDTO model);
        Task<OperationResult> UpdatePartisipantStatus(ChangeStatusDTO model);
        Task<OperationResult> DeletePartisipant(DeletePartisipantDTO model);
    }
}
