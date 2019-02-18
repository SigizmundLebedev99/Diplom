using System.Threading.Tasks;

namespace TeamEdge.BusinessLogicLayer.Interfaces
{
    public interface ISubscribeService
    {
        Task Subscribe(int userId, int workItemId);
        Task Desubscribe(int userId, int workItemId);
    }
}
